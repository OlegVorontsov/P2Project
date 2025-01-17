using P2Project.Core.Dtos.Common;
using P2Project.Core.Dtos.Volunteers;
using P2Project.Core.Interfaces.Commands;

namespace P2Project.Volunteers.Application.Commands.Create
{
    public record CreateCommand(
                  FullNameDto FullName,
                  VolunteerInfoDto VolunteerInfo,
                  string Gender,
                  string Email,
                  string? Description,
                  IEnumerable<PhoneNumberDto>? PhoneNumbers,
                  IEnumerable<SocialNetworkDto>? SocialNetworks,
                  IEnumerable<AssistanceDetailDto>? AssistanceDetails) : ICommand;
}
