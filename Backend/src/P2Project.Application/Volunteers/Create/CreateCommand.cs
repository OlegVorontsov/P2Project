using P2Project.Application.Shared;
using P2Project.Application.Shared.Dtos;

namespace P2Project.Application.Volunteers.CreateVolunteer
{
    public record CreateCommand(
                  FullNameDto FullName,
                  int Age,
                  string Gender,
                  string Email,
                  string? Description,
                  IEnumerable<PhoneNumberDto> PhoneNumbers,
                  IEnumerable<SocialNetworkDto>? SocialNetworks,
                  IEnumerable<AssistanceDetailDto>? AssistanceDetails) : ICommand;
}
