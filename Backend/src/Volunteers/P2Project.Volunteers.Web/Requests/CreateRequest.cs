using P2Project.Core.Dtos.Common;
using P2Project.Core.Dtos.Volunteers;
using P2Project.Volunteers.Application.Commands.Create;

namespace P2Project.Volunteers.Web.Requests;

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
