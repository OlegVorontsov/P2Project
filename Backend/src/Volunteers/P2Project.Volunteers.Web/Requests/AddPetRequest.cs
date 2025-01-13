using P2Project.Core.Dtos.Common;
using P2Project.Core.Dtos.Pets;
using P2Project.Volunteers.Application.Commands.AddPet;

namespace P2Project.Volunteers.Web.Requests;

public record AddPetRequest(
    Guid SpeciesId,
    Guid BreedId,
    string NickName,
    string? Description,
    string Color,
    HealthInfoDto HealthInfo,
    AddressDto Address,
    PhoneNumberDto OwnerPhoneNumber,
    DateOnly DateOfBirth,
    string AssistanceStatus,
    AssistanceDetailDto AssistanceDetail)

{
    public AddPetCommand ToCommand(Guid volunteerId) =>
        new(volunteerId,
            SpeciesId,
            BreedId,
            NickName,
            Description,
            Color,
            HealthInfo,
            Address,
            OwnerPhoneNumber,
            DateOfBirth,
            AssistanceStatus,
            AssistanceDetail);
}

