using CSharpFunctionalExtensions;
using FilesService.Core.Interfaces;
using FilesService.Core.Requests.AmazonS3;
using FilesService.Core.Responses.AmazonS3;
using FluentValidation;
using Microsoft.Extensions.Logging;
using P2Project.Core.Extensions;
using P2Project.Core.Interfaces.Commands;
using P2Project.SharedKernel.Errors;
using P2Project.SharedKernel.IDs;

namespace P2Project.Volunteers.Application.Commands.SetAvatar.UploadAvatar;

public class UploadAvatarHandler :
    ICommandHandler<UploadFileResponse, UploadAvatarCommand>
{
    private readonly IValidator<UploadAvatarCommand> _validator;
    private readonly IVolunteersRepository _volunteersRepository;
    private readonly IFilesHttpClient _httpClient;
    private readonly ILogger<UploadAvatarHandler> _logger;

    public UploadAvatarHandler(
        IValidator<UploadAvatarCommand> validator,
        IVolunteersRepository volunteersRepository,
        IFilesHttpClient httpClient,
        ILogger<UploadAvatarHandler> logger)
    {
        _validator = validator;
        _volunteersRepository = volunteersRepository;
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<Result<UploadFileResponse, ErrorList>> Handle(
        UploadAvatarCommand command,
        CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(
            command, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.ToErrorList();
        
        var volunteerId = VolunteerId.Create(command.VolunteerId);

        var volunteerResult = await _volunteersRepository.GetById(
            volunteerId, cancellationToken);
        if (volunteerResult.IsFailure)
            return volunteerResult.Error.ToErrorList();
        
        var petId = PetId.Create(command.PetId);
        var petResult = volunteerResult.Value.Pets.FirstOrDefault(p => p.Id == petId);
        if(petResult is null)
            return Errors.General.NotFound(command.PetId).ToErrorList();
        
        var startMultipartUploadResponse = await _httpClient
            .StartMultipartUpload(
                command.StartMultipartUploadRequest,
                cancellationToken);
        if (startMultipartUploadResponse.IsFailure)
            return Errors.General.Failure(startMultipartUploadResponse.Error).ToErrorList();
        
        var getPreSignedUrlResponse = await _httpClient
            .UploadPresignedPartUrl(
                startMultipartUploadResponse.Value.Key,
                new UploadPresignedPartUrlRequest(
                    command.StartMultipartUploadRequest.BucketName,
                    startMultipartUploadResponse.Value.UploadId,
                    1),
                cancellationToken);
        if (getPreSignedUrlResponse.IsFailure)
            return Errors.General.Failure(getPreSignedUrlResponse.Error).ToErrorList();
        
        var uploadFilResponse = await _httpClient
            .UploadFileAsync(
                getPreSignedUrlResponse.Value.Url,
                command.File,
                command.StartMultipartUploadRequest.ContentType,
                cancellationToken);
        if (uploadFilResponse.IsFailure)
            return Errors.General.Failure("Fail to upload file").ToErrorList();

        var result = new UploadFileResponse(
            startMultipartUploadResponse.Value.Key,
            startMultipartUploadResponse.Value.UploadId,
            uploadFilResponse.Value);
        
        _logger.LogInformation(
            "Uploaded avatar file for volunteer's {volunteerId} pet {petId}",
            command.VolunteerId,
            command.PetId);
        
        return result;
    }
}