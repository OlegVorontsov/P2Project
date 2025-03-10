﻿using CSharpFunctionalExtensions;
using FilesService.Core.Models;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using P2Project.Core;
using P2Project.Core.Extensions;
using P2Project.Core.Files;
using P2Project.Core.Files.Models;
using P2Project.Core.Interfaces;
using P2Project.Core.Interfaces.Commands;
using P2Project.SharedKernel;
using P2Project.SharedKernel.Errors;
using P2Project.SharedKernel.IDs;
using P2Project.SharedKernel.ValueObjects;
using P2Project.Volunteers.Domain.ValueObjects.Pets;
using FileData = P2Project.Core.Files.Models.FileData;

namespace P2Project.Volunteers.Application.Commands.AddPetPhotos
{
    public class AddPetPhotosHandler :
        ICommandHandler<Guid, AddPetPhotosCommand>
    {
        private readonly IValidator<AddPetPhotosCommand> _validator;
        private readonly IFileProvider _fileProvider;
        private readonly IVolunteersRepository _volunteersRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<AddPetPhotosHandler> _logger;
        private readonly IMessageQueue<IEnumerable<FileInfoDto>> _messageQueue;

        public AddPetPhotosHandler(
            IValidator<AddPetPhotosCommand> validator,
            IFileProvider fileProvider,
            IVolunteersRepository volunteersRepository,
            [FromKeyedServices(Modules.Volunteers)] IUnitOfWork unitOfWork,
            ILogger<AddPetPhotosHandler> logger,
            IMessageQueue<IEnumerable<FileInfoDto>> messageQueue)
        {
            _validator = validator;
            _fileProvider = fileProvider;
            _volunteersRepository = volunteersRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _messageQueue = messageQueue;
        }

        public async Task<Result<Guid, ErrorList>> Handle(
            AddPetPhotosCommand photosCommand,
            CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(
                photosCommand, cancellationToken);
            if (validationResult.IsValid == false)
                return validationResult.ToErrorList();
            
            var transaction = await _unitOfWork.BeginTransaction(cancellationToken);

            var volunteerId = VolunteerId.Create(photosCommand.VolunteerId);

            var volunteerResult = await _volunteersRepository.GetById(
                volunteerId, cancellationToken);
            if (volunteerResult.IsFailure)
                return Errors.General.NotFound(photosCommand.VolunteerId).ToErrorList();

            var petId = PetId.Create(photosCommand.PetId);

            var petResult = volunteerResult.Value.GetPetById(petId);
            if (petResult.IsFailure)
                return Errors.General.NotFound(photosCommand.PetId).ToErrorList();

            List<FileData> filesData = [];

            try
            {
                foreach (var file in photosCommand.Files)
                {
                    var extension = Path.GetExtension(file.FileName);

                    var filePath = FilePath.Create(Guid.NewGuid(), extension);
                    if (filePath.IsFailure)
                        return filePath.Error.ToErrorList();

                    var fileInfo = new FileInfoDto(
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
                        filesData.Select(f => f.FileInfoDto), cancellationToken);

                    return filePathsResult.Error.ToErrorList();
                }

                var petPhotos = filePathsResult.Value
                    .Select(f => MediaFile.Create(
                        Constants.BUCKET_NAME_PHOTOS, f.Path, false).Value)
                    .ToList();

                petResult.Value.UpdatePhotos(petPhotos);

                var id = _volunteersRepository.Save(volunteerResult.Value);
                await _unitOfWork.SaveChanges(cancellationToken);
                
                transaction.Commit();

                _logger.LogInformation(
                    "Photos for pet with ID: {petId} updated successfully",
                    petResult.Value.Id.Value);

                return petResult.Value.Id.Value;
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error occured while updating pet (id = {petId}) photos to volunteer with id {id}",
                    petId,
                    photosCommand.VolunteerId);

                transaction.Rollback();

                return Error.Failure("volunteer.pet.photos.failure",
                    "Error occured while uploading pet photos").ToErrorList();
            }
        }
    }
}
