using Amazon.S3;
using Amazon.S3.Model;
using CSharpFunctionalExtensions;
using FilesService.Application.Interfaces;
using FilesService.Core.Dtos;
using FilesService.Core.Interfaces;
using FilesService.Core.Models;
using FilesService.Core.Requests.AmazonS3;
using FilesService.Core.Requests.Minio;
using FilesService.Core.ValueObjects;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using P2Project.Core;
using P2Project.Core.Extensions;
using P2Project.Core.Interfaces;
using P2Project.Core.Interfaces.Commands;
using P2Project.SharedKernel;
using P2Project.SharedKernel.Errors;
using P2Project.SharedKernel.IDs;

namespace P2Project.Volunteers.Application.Commands.AddPetPhotos
{
    public class AddPetPhotosHandler :
        ICommandHandler<List<Guid>, AddPetPhotosCommand>
    {
        private readonly IValidator<AddPetPhotosCommand> _validator;
        private readonly IFileProvider _fileProvider;
        private readonly IVolunteersRepository _volunteersRepository;
        private readonly IFilesHttpClient _httpClient;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<AddPetPhotosHandler> _logger;
        private readonly IMessageQueue<IEnumerable<FileInfoDto>> _messageQueue;

        public AddPetPhotosHandler(
            IValidator<AddPetPhotosCommand> validator,
            IFileProvider fileProvider,
            IVolunteersRepository volunteersRepository,
            IFilesHttpClient httpClient,
            [FromKeyedServices(Modules.Volunteers)] IUnitOfWork unitOfWork,
            ILogger<AddPetPhotosHandler> logger,
            IMessageQueue<IEnumerable<FileInfoDto>> messageQueue)
        {
            _validator = validator;
            _fileProvider = fileProvider;
            _volunteersRepository = volunteersRepository;
            _httpClient = httpClient;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _messageQueue = messageQueue;
        }

        public async Task<Result<List<Guid>, ErrorList>> Handle(
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

            List<UploadFileKeyRequest> uploadFileRequests = [];
            List<FileInfoDto> fileInfoDtos = [];
            try
            {
                foreach (var file in photosCommand.Files)
                {
                    var extension = Path.GetExtension(file.FileName);
                    var newfileKey = Guid.NewGuid();

                    var filePath = FilePath.Create(newfileKey, extension);
                    if (filePath.IsFailure)
                        return Errors.General.Failure(filePath.Error.Message).ToErrorList();
                    
                    fileInfoDtos.Add(new FileInfoDto(filePath.Value, Constants.BUCKET_NAME_PHOTOS));

                    var fileRequestDto = new FileRequestDto(
                        newfileKey,
                        Constants.BUCKET_NAME_PHOTOS,
                        file.ContentType,
                        file.Lenght
                        );

                    var uploadFileRequest = new UploadFileKeyRequest(
                        file.Stream, fileRequestDto);

                    uploadFileRequests.Add(uploadFileRequest);
                }
                
                var filePathsResult = await _fileProvider.UploadFiles(
                    uploadFileRequests, cancellationToken);
                if (filePathsResult.IsFailure)
                {
                    await _messageQueue.WriteAsync(
                        fileInfoDtos, cancellationToken);

                    return Errors.General.Failure(filePathsResult.Error.Message).ToErrorList();
                }

                var petPhotos = uploadFileRequests
                    .Select(f => MediaFile.Create(
                        f.FileRequestDto.BucketName,
                        f.FileRequestDto.FileKey.ToString(),
                        false).Value)
                    .ToList();

                petResult.Value.UpdatePhotos(petPhotos);

                var id = _volunteersRepository.Save(volunteerResult.Value);
                await _unitOfWork.SaveChanges(cancellationToken);

                var saveFilesDataByKeysResponse = await _httpClient
                    .SaveFilesDataByKeys(
                        new SaveFilesDataByKeysRequest(
                            uploadFileRequests.Select(u => u.FileRequestDto).ToList()),
                        cancellationToken);
                if (saveFilesDataByKeysResponse.IsFailure)
                    return Errors.General.Failure().ToErrorList();
                
                transaction.Commit();

                _logger.LogInformation(
                    "Photos for pet with ID: {petId} updated successfully",
                    petResult.Value.Id.Value);

                return saveFilesDataByKeysResponse.Value;
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
