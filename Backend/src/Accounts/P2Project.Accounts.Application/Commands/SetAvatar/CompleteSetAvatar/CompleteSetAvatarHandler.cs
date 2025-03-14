using CSharpFunctionalExtensions;
using FilesService.Core.Interfaces;
using FilesService.Core.Models;
using FilesService.Core.Requests.AmazonS3;
using FilesService.Core.Responses.AmazonS3;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using P2Project.Accounts.Domain;
using P2Project.Core;
using P2Project.Core.Extensions;
using P2Project.Core.Interfaces;
using P2Project.Core.Interfaces.Commands;
using P2Project.SharedKernel.Errors;

namespace P2Project.Accounts.Application.Commands.SetAvatar.CompleteSetAvatar;

public class CompleteSetAvatarHandler :
    ICommandHandler<FileLocationResponse, CompleteSetAvatarCommand>
{
    private readonly IValidator<CompleteSetAvatarCommand> _validator;
    private readonly UserManager<User> _userManager;
    private readonly IFilesHttpClient _httpClient;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CompleteSetAvatarHandler> _logger;

    public CompleteSetAvatarHandler(
        IValidator<CompleteSetAvatarCommand> validator,
        UserManager<User> userManager,
        IFilesHttpClient httpClient,
        [FromKeyedServices(Modules.Accounts)] IUnitOfWork unitOfWork,
        ILogger<CompleteSetAvatarHandler> logger)
    {
        _validator = validator;
        _userManager = userManager;
        _httpClient = httpClient;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<FileLocationResponse, ErrorList>> Handle(
        CompleteSetAvatarCommand command,
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
        
        var completeMultipartUploadResponse = await _httpClient
            .CompleteMultipartUpload(
                command.Key,
                new CompleteMultipartRequest(
                    command.BucketName,
                    command.UploadId,
                    [new PartETagInfo(1, command.ETag)]),
                cancellationToken);
        
        if (completeMultipartUploadResponse.IsFailure)
            return Errors.General.Failure(
                completeMultipartUploadResponse.Error).ToErrorList();

        var result = completeMultipartUploadResponse.Value;
        
        var mediaFile = MediaFile.Create(
            command.BucketName,
            completeMultipartUploadResponse.Value.Location,
            false).Value;
        
        var transaction = await _unitOfWork.BeginTransaction(cancellationToken);
        user.SetAvatar(mediaFile);
        await _unitOfWork.SaveChanges(cancellationToken);
        transaction.Commit();
        
        _logger.LogInformation("Set avatar for user with id {userId}",
            command.UserId);
        
        return result;
    }
}