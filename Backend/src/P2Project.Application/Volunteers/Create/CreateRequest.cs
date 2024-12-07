using P2Project.Application.Shared.Dtos;

namespace P2Project.Application.Volunteers.CreateVolunteer;

public record CreateRequest(
            FullNameDto FullName,
            int Age,
            string Gender,
            string Email,
            string? Description,
            IEnumerable<PhoneNumberDto> PhoneNumbers,
            IEnumerable<SocialNetworkDto>? SocialNetworks,
            IEnumerable<AssistanceDetailDto>? AssistanceDetails)
{
    public CreateCommand ToCommand() =>
        new(FullName,
            Age,
            Gender,
            Email,
            Description,
            PhoneNumbers,
            SocialNetworks,
            AssistanceDetails);
}
