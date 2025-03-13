using CSharpFunctionalExtensions;
using FilesService.Core.Interfaces;
using FilesService.Core.Requests.AmazonS3;
using FilesService.Core.Responses.AmazonS3;
using FluentValidation;
using P2Project.Core.Extensions;
using P2Project.Core.Interfaces.Commands;
using P2Project.SharedKernel.Errors;

namespace P2Project.Accounts.Application.Commands.SetAvatar.UploadAvatar;

public class UploadAvatarHandler :
    ICommandHandler<UploadFileResponse, UploadAvatarCommand>
{
    private readonly IValidator<UploadAvatarCommand> _validator;
    private readonly IFilesHttpClient _httpClient;

    public UploadAvatarHandler(
        IValidator<UploadAvatarCommand> validator,
        IFilesHttpClient httpClient)
    {
        _validator = validator;
        _httpClient = httpClient;
    }

    public async Task<Result<UploadFileResponse, ErrorList>> Handle(
        UploadAvatarCommand command,
        CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(
            command, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.ToErrorList();
        
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
        
        return result;
    }
}