using P2Project.Application.Shared.Dtos.Common;
using P2Project.Application.Shared.Dtos.Pets;
using P2Project.Application.Volunteers.Commands.UpdatePet;

namespace P2Project.API.Controllers.Volunteers.Requests;

public record UpdatePetRequest(
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
    AssistanceDetailDto AssistanceDetails)
{
    public UpdatePetCommand ToCommand(Guid volunteerId, Guid petId) =>
        new(volunteerId,
            petId,
            SpeciesId,
            BreedId,
            NickName,
            Description,
            Color,
            HealthInfo,
            Address,
            OwnerPhoneNumber,
            BirthDate,
            AssistanceStatus,
            AssistanceDetails);
}