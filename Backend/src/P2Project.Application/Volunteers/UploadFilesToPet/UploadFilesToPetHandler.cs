using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using P2Project.Application.FileProvider.Models;
using P2Project.Application.Species;
using P2Project.Domain.PetManagment.ValueObjects;
using P2Project.Domain.Shared.IDs;
using P2Project.Domain.Shared;
using P2Project.Application.FileProvider;
using P2Project.Application.Shared;

namespace P2Project.Application.Volunteers.UploadFilesToPet
{
    public class UploadFilesToPetHandler
    {
        private const string BUCKET_NAME = "photos";

        private readonly IFileProvider _fileProvider;
        private readonly IVolunteersRepository _volunteersRepository;
        private readonly ISpeciesRepository _speciesRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<UploadFilesToPetHandler> _logger;

        public UploadFilesToPetHandler(
            IFileProvider fileProvider,
            IVolunteersRepository volunteersRepository,
            ISpeciesRepository speciesRepository,
            IUnitOfWork unitOfWork,
            ILogger<UploadFilesToPetHandler> logger)
        {
            _fileProvider = fileProvider;
            _volunteersRepository = volunteersRepository;
            _speciesRepository = speciesRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Result<Guid, ErrorList>> Handle(
            UploadFilesToPetCommand command,
            CancellationToken cancellationToken)
        {
            var volunteerId = VolunteerId.Create(command.VolunteerId);

            var volunteerResult = await _volunteersRepository.GetById(
                volunteerId, cancellationToken);
            if (volunteerResult.IsFailure)
                return volunteerResult.Error.ToErrorList();

            var petId = PetId.Create(command.PetId);

            var petResult = volunteerResult.Value.GetPetById(petId);
            if (petResult.IsFailure)
                return petResult.Error.ToErrorList();

            List<FileData> filesData = [];
            foreach (var file in command.Files)
            {
                var extension = Path.GetExtension(file.FileName);

                var filePath = FilePath.Create(Guid.NewGuid(), extension);
                if (filePath.IsFailure)
                    return filePath.Error.ToErrorList();

                var fileData = new FileData(
                    file.Stream, filePath.Value, BUCKET_NAME);

                filesData.Add(fileData);
            }

            var filePathsResult = await _fileProvider.UploadFiles(
                filesData, cancellationToken);
            if (filePathsResult.IsFailure)
                return filePathsResult.Error.ToErrorList();

            var petPhotos = filePathsResult.Value
                .Select(f => PetPhoto.Create(f.Path, false).Value)
                .ToList();

            petResult.Value.UpdatePhotos(petPhotos);
            await _unitOfWork.SaveChanges(cancellationToken);

            _logger.LogInformation(
                "Uploaded photos to pet - {petId}",
                petResult.Value.Id.Value);

            return petResult.Value.Id.Value;
        }
    }
}
