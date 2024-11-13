using P2Project.Application.Dtos;

namespace P2Project.Application.Volunteers.CreateVolunteer
{
    public record CreateVolunteerRequest(
            FullNameDto FullName,
            int Age,
            string Gender,
            string Email,
            string? Description,
            IEnumerable<PhoneNumberDto> PhoneNumbers,
            IEnumerable<SocialNetworkDto>? SocialNetworks,
            IEnumerable<AssistanceDetailDto>? AssistanceDetails);
}
