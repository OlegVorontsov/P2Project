using P2Project.Application.Interfaces;
using P2Project.Application.Interfaces.Commands;
using P2Project.Application.Shared;
using P2Project.Application.Shared.Dtos;

namespace P2Project.Application.Volunteers.Commands.AddPet
{
    public record AddPetCommand(
        Guid VolunteerId,
        string NickName,
        string Species,
        string Breed,
        string? Description,
        string Color,
        HealthInfoDto HealthInfo,
        AddressDto Address,
        PhoneNumberDto OwnerPhoneNumber,
        DateOnly BirthDate,
        string AssistanceStatus,
        AssistanceDetailDto AssistanceDetail) : ICommand;
}
