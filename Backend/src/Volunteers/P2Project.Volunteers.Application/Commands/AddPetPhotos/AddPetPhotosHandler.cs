using CSharpFunctionalExtensions;
using FilesService.Core.Dtos;
using FilesService.Core.Interfaces;
using FilesService.Core.Models;
using FilesService.Core.Responses.AmazonS3;
using FilesService.Core.ValueObjects;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using P2Project.Core;
using P2Project.Core.Extensions;
using P2Project.Core.Interfaces;
using P2Project.Core.Interfaces.Commands;
using P2Project.SharedKernel.Errors;
using P2Project.SharedKernel.IDs;

namespace P2Project.Volunteers.Application.Commands.AddPetPhotos
{
    public class AddPetPhotosHandler :
        ICommandHandler<Guid, AddPetPhotosCommand>
    {
        private readonly IValidator<AddPetPhotosCommand> _validator;
        private readonly IFilesHttpClient _httpClient;
        private readonly IVolunteersRepository _volunteersRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<AddPetPhotosHandler> _logger;
        private readonly IMessageQueue<IEnumerable<FileInfoDto>> _messageQueue;

        public AddPetPhotosHandler(
            IValidator<AddPetPhotosCommand> validator,
            IFilesHttpClient httpClient,
            IVolunteersRepository volunteersRepository,
            [FromKeyedServices(Modules.Volunteers)] IUnitOfWork unitOfWork,
            ILogger<AddPetPhotosHandler> logger,
            IMessageQueue<IEnumerable<FileInfoDto>> messageQueue)
        {
            _validator = validator;
            _httpClient = httpClient;
            _volunteersRepository = volunteersRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _messageQueue = messageQueue;
        }

        public async Task<Result<Guid, ErrorList>> Handle(
            AddPetPhotosCommand photosCommand,
            CancellationToken cancellationToken)
        {
            // метод связан с CompleteMultipartUpload
            
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

            List<UploadPartFileResponse> uploadFileResponses = [];

            try
            {
                foreach (var request in photosCommand.Requests)
                {
                    var response = await _httpClient.StartMultipartUpload(
                        request, cancellationToken);
                    
                    if (response.IsFailure)
                    {
                        var extension = Path.GetExtension(request.FileName);

                        var filePathResult = FilePath.Create(
                            Guid.NewGuid(), extension);
                        
                        if (filePathResult.IsFailure)
                            return Errors.General.Failure(filePathResult.Error.Message).ToErrorList();

                        var fileInfoDto = new FileInfoDto(
                            filePathResult.Value, request.BucketName);
                        
                        await _messageQueue.WriteAsync([fileInfoDto], cancellationToken);
                        
                        return Errors.General.Failure(request.FileName).ToErrorList();
                    }
                    
                    uploadFileResponses.Add(response.Value);
                }
                
                var petPhotos = photosCommand.Requests
                    .Select(r => MediaFile.Create(
                        r.BucketName, r.FileName, false).Value)
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
