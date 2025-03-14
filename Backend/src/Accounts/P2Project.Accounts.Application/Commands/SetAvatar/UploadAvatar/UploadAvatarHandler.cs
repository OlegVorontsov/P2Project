using CSharpFunctionalExtensions;
using FilesService.Core.Interfaces;
using FilesService.Core.Requests.AmazonS3;
using FilesService.Core.Responses.AmazonS3;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using P2Project.Accounts.Domain;
using P2Project.Core.Extensions;
using P2Project.Core.Interfaces.Commands;
using P2Project.SharedKernel.Errors;

namespace P2Project.Accounts.Application.Commands.SetAvatar.UploadAvatar;

public class UploadAvatarHandler :
    ICommandHandler<UploadFileResponse, UploadAvatarCommand>
{
    private readonly IValidator<UploadAvatarCommand> _validator;
    private readonly UserManager<User> _userManager;
    private readonly IFilesHttpClient _httpClient;
    private readonly ILogger<UploadAvatarHandler> _logger;

    public UploadAvatarHandler(
        IValidator<UploadAvatarCommand> validator,
        UserManager<User> userManager,
        IFilesHttpClient httpClient,
        ILogger<UploadAvatarHandler> logger)
    {
        _validator = validator;
        _userManager = userManager;
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
        
        var user = await _userManager.Users
            .FirstOrDefaultAsync(u => u.Id == command.UserId, cancellationToken);
        if (user == null)
            return Errors.General.NotFound(command.UserId).ToErrorList();
        
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
        
        _logger.LogInformation("Uploaded avatar file for user with id {userId}",
            command.UserId);
        
        return result;
    }
}