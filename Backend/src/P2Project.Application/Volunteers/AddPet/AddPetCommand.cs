using P2Project.Application.Shared.Dtos;

namespace P2Project.Application.Volunteers.CreatePet
{
    public record AddPetCommand(
        Guid VolunteerId,
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
        AssistanceDetailDto AssistanceDetail);
}
