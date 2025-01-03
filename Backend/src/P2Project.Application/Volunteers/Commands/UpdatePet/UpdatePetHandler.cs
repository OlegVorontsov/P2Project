using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using P2Project.Application.Extensions;
using P2Project.Application.Interfaces;
using P2Project.Application.Interfaces.Agreements;
using P2Project.Application.Interfaces.Commands;
using P2Project.Application.Interfaces.DataBase;
using P2Project.Application.Species.Commands.Create;
using P2Project.Application.Volunteers.Commands.AddPet;
using P2Project.Domain.PetManagment.ValueObjects.Common;
using P2Project.Domain.PetManagment.ValueObjects.Pets;
using P2Project.Domain.Shared.Errors;
using P2Project.Domain.Shared.IDs;

namespace P2Project.Application.Volunteers.Commands.UpdatePet;

public class UpdatePetHandler : ICommandHandler<Guid, UpdatePetCommand>
{
    private readonly IValidator<UpdatePetCommand> _validator;
    private readonly ISpeciesAgreement _speciesAgreement;
    private readonly IVolunteersRepository _volunteersRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<AddPetHandler> _petLogger;
    private readonly ILogger<CreateHandler> _speciesLogger;

    public UpdatePetHandler(
        IValidator<UpdatePetCommand> validator,
        ISpeciesAgreement speciesAgreement,
        IVolunteersRepository volunteersRepository,
        IUnitOfWork unitOfWork,
        ILogger<AddPetHandler> petLogger,
        ILogger<CreateHandler> speciesLogger)
    {
        _validator = validator;
        _speciesAgreement = speciesAgreement;
        _volunteersRepository = volunteersRepository;
        _unitOfWork = unitOfWork;
        _petLogger = petLogger;
        _speciesLogger = speciesLogger;
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
            _speciesLogger.LogInformation(
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

        _petLogger.LogInformation(
            "Succsessfully updated pet with id {petId}", petId);

        return petId.Value;
    }
}