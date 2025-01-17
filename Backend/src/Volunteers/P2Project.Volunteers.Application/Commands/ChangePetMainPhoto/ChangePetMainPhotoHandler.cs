using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using P2Project.Core;
using P2Project.Core.Extensions;
using P2Project.Core.Interfaces;
using P2Project.Core.Interfaces.Commands;
using P2Project.SharedKernel.Errors;
using P2Project.SharedKernel.IDs;
using P2Project.Volunteers.Domain.ValueObjects.Pets;

namespace P2Project.Volunteers.Application.Commands.ChangePetMainPhoto;

public class ChangePetMainPhotoHandler :
    ICommandHandler<Guid, ChangePetMainPhotoCommand>
{
    private readonly IValidator<ChangePetMainPhotoCommand> _validator;
    private readonly IVolunteersRepository _volunteersRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<ChangePetMainPhotoHandler> _logger;

    public ChangePetMainPhotoHandler(
        IValidator<ChangePetMainPhotoCommand> validator,
        IVolunteersRepository volunteersRepository,
        [FromKeyedServices(Modules.Volunteers)] IUnitOfWork unitOfWork,
        ILogger<ChangePetMainPhotoHandler> logger)
    {
        _validator = validator;
        _volunteersRepository = volunteersRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        ChangePetMainPhotoCommand command,
        CancellationToken cancellationToken = default)
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
        var newMainPhoto = PetPhoto.Create(command.ObjectName, true).Value;
        
        var changeResult = volunteerResult.Value
            .ChangePetMainPhoto(petId, newMainPhoto);
        if (changeResult.IsFailure)
            return changeResult.Error.ToErrorList();
        
        await _unitOfWork.SaveChanges(cancellationToken);

        _logger.LogInformation(
            "Successfully changed volunteer's (id = {vId}) pet's (id = {pId}) main photo {newMainPhoto}",
            volunteerId,
            petId,
            command.ObjectName);

        return petId.Value;
    }
}