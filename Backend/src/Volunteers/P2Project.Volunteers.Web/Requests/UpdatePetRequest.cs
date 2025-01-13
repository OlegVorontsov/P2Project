using P2Project.Core.Dtos.Common;
using P2Project.Core.Dtos.Pets;
using P2Project.Volunteers.Application.Commands.UpdatePet;

namespace P2Project.Volunteers.Web.Requests;

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