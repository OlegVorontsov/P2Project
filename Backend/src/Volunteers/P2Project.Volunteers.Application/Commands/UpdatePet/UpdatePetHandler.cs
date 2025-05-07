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
using P2Project.SharedKernel.ValueObjects;
using P2Project.Species.Agreements;
using P2Project.Volunteers.Application.Commands.AddPet;
using P2Project.Volunteers.Application.Commands.Create;
using P2Project.Volunteers.Application.Interfaces;
using P2Project.Volunteers.Domain.Events;
using P2Project.Volunteers.Domain.ValueObjects.Pets;

namespace P2Project.Volunteers.Application.Commands.UpdatePet;

public class UpdatePetHandler : ICommandHandler<Guid, UpdatePetCommand>
{
    private readonly IValidator<UpdatePetCommand> _validator;
    private readonly ISpeciesAgreement _speciesAgreement;
    private readonly IVolunteersRepository _volunteersRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<AddPetHandler> _petLogger;
    private readonly IPublisher _publisher;
    
    public UpdatePetHandler(
        IValidator<UpdatePetCommand> validator,
        ISpeciesAgreement speciesAgreement,
        IVolunteersRepository volunteersRepository,
        [FromKeyedServices(Modules.Volunteers)] IUnitOfWork unitOfWork,
        ILogger<AddPetHandler> petLogger,
        IPublisher publisher)
    {
        _validator = validator;
        _speciesAgreement = speciesAgreement;
        _volunteersRepository = volunteersRepository;
        _unitOfWork = unitOfWork;
        _petLogger = petLogger;
        _publisher = publisher;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        UpdatePetCommand command,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(
            command, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.ToErrorList();
        
        var speciesExistsResult = await _speciesAgreement.SpeciesAndBreedExists(
            command.SpeciesId, command.BreedId, cancellationToken);
        if (speciesExistsResult.IsFailure)
        {
            _petLogger.LogInformation(
                "Tried to create pet with unexisting species or breed id");
            return speciesExistsResult.Error.ToErrorList();
        }
        
        var volunteerId = VolunteerId.Create(command.VolunteerId);

        var volunteerResult = await _volunteersRepository.GetById(
            volunteerId, cancellationToken);
        if (volunteerResult.IsFailure)
            return volunteerResult.Error.ToErrorList();
        
        var petId = PetId.Create(command.PetId);
        var nicKName = NickName.Create(command.NickName).Value;
        var specieBreed = new SpeciesBreed(
            SpeciesId.Create(command.SpeciesId), command.BreedId);
        var description = Description.Create(command.Description).Value;
        var color = Color.Create(command.Color).Value;
        var healthInfo = HealthInfo.Create(
            command.HealthInfo.Weight,
            command.HealthInfo.Height,
            command.HealthInfo.IsCastrated,
            command.HealthInfo.IsVaccinated,
            command.HealthInfo.HealthDescription).Value;
        var address = Address.Create(
            command.Address.Region,
            command.Address.City,
            command.Address.Street,
            command.Address.House,
            command.Address.Floor,
            command.Address.Apartment).Value;
        var ownerPhoneNumber = PhoneNumber.Create(
            command.OwnerPhoneNumber.Value,
            command.OwnerPhoneNumber.IsMain).Value;
        var assistanceStatus = AssistanceStatus.Create(
            command.AssistanceStatus).Value;
        var assistanceDetails = new List<AssistanceDetail>();
        if (command.AssistanceDetails != null)
        {
            var detail = AssistanceDetail.Create(
                command.AssistanceDetails.Name,
                command.AssistanceDetails.Description,
                command.AssistanceDetails.AccountNumber).Value;
            assistanceDetails.AddRange([detail]);
        }
        var petAssistanceDetails = assistanceDetails;

        var updateResult = volunteerResult.Value.UpdatePet(
            petId,
            nicKName,
            specieBreed,
            description,
            color,
            healthInfo,
            address,
            ownerPhoneNumber,
            command.BirthDate,
            assistanceStatus,
            petAssistanceDetails);
        if (updateResult.IsFailure)
            return updateResult.Error.ToErrorList();

        await _unitOfWork.SaveChanges(cancellationToken);

        await _publisher.Publish(new PetWasChangedEvent(), cancellationToken);

        _petLogger.LogInformation(
            "Successfully updated pet with id {petId}", petId);

        return petId.Value;
    }
}