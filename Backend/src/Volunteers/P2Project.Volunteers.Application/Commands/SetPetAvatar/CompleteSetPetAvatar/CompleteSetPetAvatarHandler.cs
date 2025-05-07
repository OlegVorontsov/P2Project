using CSharpFunctionalExtensions;
using FilesService.Core.Interfaces;
using FilesService.Core.Models;
using FilesService.Core.Requests.AmazonS3;
using FilesService.Core.Responses.AmazonS3;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using P2Project.Core;
using P2Project.Core.Extensions;
using P2Project.Core.Interfaces;
using P2Project.Core.Interfaces.Commands;
using P2Project.SharedKernel.Errors;
using P2Project.SharedKernel.IDs;
using P2Project.Volunteers.Application.Interfaces;
using P2Project.Volunteers.Domain.Events;

namespace P2Project.Volunteers.Application.Commands.SetPetAvatar.CompleteSetPetAvatar;

public class CompleteSetPetAvatarHandler :
    ICommandHandler<FileLocationResponse, CompleteSetPetAvatarCommand>
{
    private readonly IValidator<CompleteSetPetAvatarCommand> _validator;
    private readonly IVolunteersRepository _volunteersRepository;
    private readonly IFilesHttpClient _httpClient;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CompleteSetPetAvatarHandler> _logger;
    private readonly IPublisher _publisher;

    public CompleteSetPetAvatarHandler(
        IValidator<CompleteSetPetAvatarCommand> validator,
        IVolunteersRepository volunteersRepository,
        IFilesHttpClient httpClient,
        [FromKeyedServices(Modules.Volunteers)] IUnitOfWork unitOfWork,
        ILogger<CompleteSetPetAvatarHandler> logger,
        IPublisher publisher)
    {
        _validator = validator;
        _volunteersRepository = volunteersRepository;
        _httpClient = httpClient;
        _unitOfWork = unitOfWork;
        _logger = logger;
        _publisher = publisher;
    }

    public async Task<Result<FileLocationResponse, ErrorList>> Handle(
        CompleteSetPetAvatarCommand command,
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
                command.FileName,
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
            completeMultipartUploadResponse.Value.FileId,
            completeMultipartUploadResponse.Value.FilePath,
            false).Value;
        
        var transaction = await _unitOfWork.BeginTransaction(cancellationToken);
        petResult.SetAvatar(mediaFile);
        await _unitOfWork.SaveChanges(cancellationToken);
        transaction.Commit();

        await _publisher.Publish(new PetWasChangedEvent(), cancellationToken);

        _logger.LogInformation("Set avatar for volunteer's {volunteerId} pet {petId}",
            command.VolunteerId,
            command.PetId);
        
        return result;
    }
}