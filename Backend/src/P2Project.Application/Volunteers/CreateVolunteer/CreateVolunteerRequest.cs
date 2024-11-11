using P2Project.Application.Dtos;

namespace P2Project.Application.Volunteers.CreateVolunteer
{
    public record CreateVolunteerRequest(FullNameDto fullName,
                                         int age,
                                         string gender,
                                         string Email,
                                         string? Description,
                                         IEnumerable<PhoneNumberDto> phoneNumbers,
                                         IEnumerable<SocialNetworkDto>? socialNetworks,
                                         IEnumerable<AssistanceDetailDto>? assistanceDetails);
}
