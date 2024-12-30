using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using P2Project.Application.Extensions;
using P2Project.Application.Interfaces;
using P2Project.Application.Interfaces.Commands;
using P2Project.Application.Interfaces.DataBase;
using P2Project.Domain.PetManagment.ValueObjects.Pets;
using P2Project.Domain.Shared.Errors;
using P2Project.Domain.Shared.IDs;

namespace P2Project.Application.Volunteers.Commands.ChangePetStatus;

public class ChangePetStatusHandler :
    ICommandHandler<Guid, ChangePetStatusCommand>
{
    private readonly IValidator<ChangePetStatusCommand> _validator;
    private readonly IVolunteersRepository _volunteersRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<ChangePetStatusHandler> _logger;

    public ChangePetStatusHandler(
        IValidator<ChangePetStatusCommand> validator,
        IVolunteersRepository volunteersRepository,
        IUnitOfWork unitOfWork,
        ILogger<ChangePetStatusHandler> logger)
    {
        _validator = validator;
        _volunteersRepository = volunteersRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        ChangePetStatusCommand command,
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
        var assistanceStatus = AssistanceStatus.Create(
            command.Status).Value;
        
        var changeResult = volunteerResult.Value
            .ChangePetStatus(petId, assistanceStatus);
        if (changeResult.IsFailure)
            return changeResult.Error.ToErrorList();

        await _unitOfWork.SaveChanges(cancellationToken);

        _logger.LogInformation(
            "Successfully changed volunteer's (id = {vId}) pet's (id = {pId}) status to {newStatus}",
            volunteerId,
            petId,
            command.Status);

        return petId.Value;
    }
}