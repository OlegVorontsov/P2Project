using P2Project.Application.Shared.Dtos;
using P2Project.Application.Shared.Dtos.Common;
using P2Project.Application.Shared.Dtos.Pets;
using P2Project.Application.Volunteers.Commands.AddPet;

namespace P2Project.API.Controllers.Volunteers.Requests;

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

