using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using P2Project.Application.Extensions;
using P2Project.Application.FileProvider;
using P2Project.Application.FileProvider.Models;
using P2Project.Application.Interfaces;
using P2Project.Application.Interfaces.Commands;
using P2Project.Application.Interfaces.Repositories;
using P2Project.Application.Messaging;
using P2Project.Domain.PetManagment.ValueObjects;
using P2Project.Domain.Shared;
using P2Project.Domain.Shared.Errors;
using P2Project.Domain.Shared.IDs;
using FileInfo = P2Project.Application.FileProvider.Models.FileInfo;

namespace P2Project.Application.Volunteers.Commands.UploadFilesToPet
{
    public class UploadFilesToPetHandler : ICommandHandler<Guid, UploadFilesToPetCommand>
    {
        private readonly IValidator<UploadFilesToPetCommand> _validator;
        private readonly IFileProvider _fileProvider;
        private readonly IVolunteersRepository _volunteersRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<UploadFilesToPetHandler> _logger;
        private readonly IMessageQueue<IEnumerable<FileInfo>> _messageQueue;

        public UploadFilesToPetHandler(
            IValidator<UploadFilesToPetCommand> validator,
            IFileProvider fileProvider,
            IVolunteersRepository volunteersRepository,
            IUnitOfWork unitOfWork,
            ILogger<UploadFilesToPetHandler> logger,
            IMessageQueue<IEnumerable<FileInfo>> messageQueue)
        {
            _validator = validator;
            _fileProvider = fileProvider;
            _volunteersRepository = volunteersRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _messageQueue = messageQueue;
        }

        public async Task<Result<Guid, ErrorList>> Handle(
            UploadFilesToPetCommand command,
            CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(
                          command,
                          cancellationToken);
            if (validationResult.IsValid == false)
                return validationResult.ToErrorList();

            var volunteerId = VolunteerId.Create(command.VolunteerId);

            var volunteerResult = await _volunteersRepository.GetById(
                volunteerId, cancellationToken);
            if (volunteerResult.IsFailure)
            {
                var error = Errors.General.NotFound(command.VolunteerId);
                return error.ToErrorList();
            }

            var petId = PetId.Create(command.PetId);

            var petResult = volunteerResult.Value.GetPetById(petId);
            if (petResult.IsFailure)
            {
                var error = Errors.General.NotFound(command.PetId);
                return error.ToErrorList();
            }

            List<FileData> filesData = [];
            foreach (var file in command.Files)
            {
                var extension = Path.GetExtension(file.FileName);

                var filePath = FilePath.Create(Guid.NewGuid(), extension);
                if (filePath.IsFailure)
                    return filePath.Error.ToErrorList();

                var fileInfo = new FileInfo(
                    filePath.Value, Constants.BUCKET_NAME_PHOTOS);

                var fileData = new FileData(
                    file.Stream, fileInfo);

                filesData.Add(fileData);
            }

            var filePathsResult = await _fileProvider.UploadFiles(
                filesData, cancellationToken);
            if (filePathsResult.IsFailure)
            {
                await _messageQueue.WriteAsync(
                    filesData.Select(f => f.FileInfo), cancellationToken);

                return filePathsResult.Error.ToErrorList();
            }

            var petPhotos = filePathsResult.Value
                .Select(f => PetPhoto.Create(f.Path).Value)
                .ToList();

            petResult.Value.UpdatePhotos(petPhotos);
            await _unitOfWork.SaveChanges(cancellationToken);

            _logger.LogInformation(
                "Uploaded photos for pet with ID: {petId}",
                petResult.Value.Id.Value);

            return petResult.Value.Id.Value;
        }
    }
}
