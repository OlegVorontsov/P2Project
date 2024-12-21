using P2Project.Application.Shared.Dtos;
using P2Project.Application.Shared.Dtos.Common;
using P2Project.Application.Shared.Dtos.Volunteers;
using P2Project.Application.Volunteers.Commands.Create;

namespace P2Project.API.Controllers.Volunteers.Requests;

public record CreateRequest(
            FullNameDto FullName,
            VolunteerInfoDto VolunteerInfo,
            string Gender,
            string Email,
            string? Description,
            IEnumerable<PhoneNumberDto> PhoneNumbers,
            IEnumerable<SocialNetworkDto>? SocialNetworks,
            IEnumerable<AssistanceDetailDto>? AssistanceDetails)
{
    public CreateCommand ToCommand() =>
        new(FullName,
            VolunteerInfo,
            Gender,
            Email,
            Description,
            PhoneNumbers,
            SocialNetworks,
            AssistanceDetails);
}
