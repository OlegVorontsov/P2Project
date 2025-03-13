using CSharpFunctionalExtensions;
using FilesService.Core.Interfaces;
using FilesService.Core.Models;
using FilesService.Core.Requests.AmazonS3;
using FluentValidation;
using P2Project.Core.Extensions;
using P2Project.Core.Interfaces.Commands;
using P2Project.SharedKernel.Errors;

namespace P2Project.Accounts.Application.Commands.SetAvatar.CompleteSetAvatar;

public class CompleteSetAvatarHandler :
    ICommandHandler<string, CompleteSetAvatarCommand>
{
    private readonly IValidator<CompleteSetAvatarCommand> _validator;
    private readonly IFilesHttpClient _httpClient;

    public CompleteSetAvatarHandler(
        IValidator<CompleteSetAvatarCommand> validator,
        IFilesHttpClient httpClient)
    {
        _validator = validator;
        _httpClient = httpClient;
    }

    public async Task<Result<string, ErrorList>> Handle(
        CompleteSetAvatarCommand command,
        CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(
            command, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.ToErrorList();
        
        var completeMultipartUploadResponse = await _httpClient
            .CompleteMultipartUpload(
                command.Key,
                new CompleteMultipartRequest(
                    command.BucketName,
                    command.UploadId,
                    [new PartETagInfo(1, command.ETag)]),
                cancellationToken);
        
        if (getPreSignedUrlResponse.IsFailure)
            return Errors.General.Failure(getPreSignedUrlResponse.Error).ToErrorList();
    }
}