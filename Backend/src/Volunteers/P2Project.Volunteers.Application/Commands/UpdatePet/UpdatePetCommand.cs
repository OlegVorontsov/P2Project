using P2Project.Core.Dtos.Common;
using P2Project.Core.Dtos.Pets;
using P2Project.Core.Interfaces.Commands;

namespace P2Project.Volunteers.Application.Commands.UpdatePet;

public record UpdatePetCommand(
    Guid VolunteerId,
    Guid PetId,
    Guid SpeciesId,
    Guid BreedId,
    string NickName,
    string? Description,
    string Color,
    HealthInfoDto HealthInfo,
    AddressDto Address,
    PhoneNumberDto OwnerPhoneNumber,
    DateOnly BirthDate,
    string AssistanceStatus,
    AssistanceDetailDto AssistanceDetails) : ICommand;