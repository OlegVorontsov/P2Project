using CSharpFunctionalExtensions;
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

namespace P2Project.Volunteers.Application.Commands.SoftDeletePet;

public class SoftDeletePetHandler :
    ICommandHandler<Guid, SoftDeletePetCommand>
{
    private readonly IValidator<SoftDeletePetCommand> _validator;
    private readonly IVolunteersRepository _volunteersRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<SoftDeletePetCommand> _logger;
    private readonly IPublisher _publisher;

    public SoftDeletePetHandler(
        IValidator<SoftDeletePetCommand> validator,
        IVolunteersRepository volunteersRepository,
        [FromKeyedServices(Modules.Volunteers)] IUnitOfWork unitOfWork,
        ILogger<SoftDeletePetCommand> logger, IPublisher publisher)
    {
        _validator = validator;
        _volunteersRepository = volunteersRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
        _publisher = publisher;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        SoftDeletePetCommand command,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(
            command, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.ToErrorList();

        var volunteerId = VolunteerId.Create(
            command.VolunteerId);

        var volunteerResult = await _volunteersRepository.GetById(
            volunteerId, cancellationToken);
        if (volunteerResult.IsFailure)
            return Errors.General.NotFound(command.VolunteerId).ToErrorList();
        
        var petId = PetId.Create(command.PetId);
        volunteerResult.Value.SoftDeletePet(petId);

        await _unitOfWork.SaveChanges(cancellationToken);

        await _publisher.Publish(new PetWasChangedEvent(), cancellationToken);

        _logger.LogInformation(
            "Softdeleted pet (id = {petId}) belonging to volunteer (id = {vid})",
            petId,
            volunteerId);

        return volunteerResult.Value.Id.Value;
    }
}