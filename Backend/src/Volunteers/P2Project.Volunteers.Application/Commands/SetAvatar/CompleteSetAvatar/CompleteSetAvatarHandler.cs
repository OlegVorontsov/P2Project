using CSharpFunctionalExtensions;
using FilesService.Core.Interfaces;
using FilesService.Core.Models;
using FilesService.Core.Requests.AmazonS3;
using FilesService.Core.Responses.AmazonS3;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using P2Project.Core;
using P2Project.Core.Extensions;
using P2Project.Core.Interfaces;
using P2Project.Core.Interfaces.Commands;
using P2Project.SharedKernel.Errors;
using P2Project.SharedKernel.IDs;

namespace P2Project.Volunteers.Application.Commands.SetAvatar.CompleteSetAvatar;

public class CompleteSetAvatarHandler :
    ICommandHandler<FileLocationResponse, CompleteSetAvatarCommand>
{
    private readonly IValidator<CompleteSetAvatarCommand> _validator;
    private readonly IVolunteersRepository _volunteersRepository;
    private readonly IFilesHttpClient _httpClient;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CompleteSetAvatarHandler> _logger;

    public CompleteSetAvatarHandler(
        IValidator<CompleteSetAvatarCommand> validator,
        IVolunteersRepository volunteersRepository,
        IFilesHttpClient httpClient,
        [FromKeyedServices(Modules.Volunteers)] IUnitOfWork unitOfWork,
        ILogger<CompleteSetAvatarHandler> logger)
    {
        _validator = validator;
        _volunteersRepository = volunteersRepository;
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
        
        var volunteerId = VolunteerId.Create(command.VolunteerId);

        var volunteerResult = await _volunteersRepository.GetById(
            volunteerId, cancellationToken);
        if (volunteerResult.IsFailure)
            return volunteerResult.Error.ToErrorList();
        
        var petId = PetId.Create(command.PetId);
        var petResult = volunteerResult.Value.Pets.FirstOrDefault(p => p.Id == petId);
        if(petResult is null)
            return Errors.General.NotFound(command.PetId).ToErrorList();
        
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
        petResult.SetAvatar(mediaFile);
        await _unitOfWork.SaveChanges(cancellationToken);
        transaction.Commit();
        
        _logger.LogInformation("Set avatar for volunteer's {volunteerId} pet {petId}",
            command.VolunteerId,
            command.PetId);
        
        return result;
    }
}