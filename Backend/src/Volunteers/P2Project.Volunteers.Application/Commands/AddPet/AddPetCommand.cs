using P2Project.Core.Dtos.Common;
using P2Project.Core.Dtos.Pets;
using P2Project.Core.Interfaces.Commands;

namespace P2Project.Volunteers.Application.Commands.AddPet
{
    public record AddPetCommand(
        Guid VolunteerId,
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
        AssistanceDetailDto AssistanceDetail) : ICommand;
}
