using P2Project.Application.Shared.Dtos;
using P2Project.Application.Volunteers.Commands.AddPet;

namespace P2Project.API.Controllers.Volunteers.Requests;

public record AddPetRequest(
    string NickName,
    string Species,
    string Breed,
    string? Description,
    string Color,
    string? HealthInfo,
    AddressDto Address,
    double Weight,
    double Height,
    PhoneNumberDto OwnerPhoneNumber,
    bool IsCastrated,
    bool IsVaccinated,
    DateOnly DateOfBirth,
    string AssistanceStatus,
    AssistanceDetailDto AssistanceDetail)

{
    public AddPetCommand ToCommand(Guid volunteerId) =>
        new(volunteerId,
            NickName,
            Species,
            Breed,
            Description,
            Color,
            HealthInfo,
            Address,
            Weight,
            Height,
            OwnerPhoneNumber,
            IsCastrated,
            IsVaccinated,
            DateOfBirth,
            AssistanceStatus,
            AssistanceDetail);
}

