using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using P2Project.Application.FileProvider.Models;
using P2Project.Application.Species;
using P2Project.Domain.PetManagment.ValueObjects;
using P2Project.Domain.Shared.IDs;
using P2Project.Domain.Shared;
using P2Project.Application.FileProvider;

namespace P2Project.Application.Volunteers.UploadFilesToPet
{
    public class UploadFilesToPetHandler
    {
        private const string BUCKET_NAME = "photos";

        private readonly IFileProvider _fileProvider;
        private readonly ILogger<UploadFilesToPetHandler> _logger;
        private readonly IVolunteersRepository _volunteersRepository;
        private readonly ISpeciesRepository _speciesRepository;

        public UploadFilesToPetHandler(
            ILogger<UploadFilesToPetHandler> logger,
            IVolunteersRepository volunteersRepository,
            ISpeciesRepository speciesRepository,
            IFileProvider fileProvider)
        {
            _logger = logger;
            _volunteersRepository = volunteersRepository;
            _speciesRepository = speciesRepository;
            _fileProvider = fileProvider;
        }

        public async Task<Result<Guid, ErrorList>> Handle(
            UploadFilesToPetCommand command,
            CancellationToken cancellationToken)
        {
            var volunteerId = VolunteerId.CreateVolunteerId(command.VolunteerId);

            var volunteerResult = await _volunteersRepository.GetById(
                volunteerId, cancellationToken);
            if (volunteerResult.IsFailure)
            {
                return volunteerResult.Error.ToErrorList();
            }

            var petId = PetId.Create(command.PetId);

            var petResult = volunteerResult.Value.GetPetById(petId);
            if (petResult.IsFailure)
            {
                return petResult.Error.ToErrorList();
            }

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
            {
                return filePathsResult.Error.ToErrorList();
            }

            var petPhotos = filePathsResult.Value
                .Select(f => PetPhoto.Create(f.Path, false).Value)
                .ToList();

            petResult.Value.UpdatePhotos(petPhotos);

            _logger.LogInformation(
                "Uploaded photos to pet - {petId}",
                petResult.Value.Id.Value);

            return petResult.Value.Id.Value;
        }
    }
}
