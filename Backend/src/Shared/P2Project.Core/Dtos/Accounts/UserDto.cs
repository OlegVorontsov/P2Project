using P2Project.Core.Dtos.Common;
using P2Project.Core.Dtos.Volunteers;

namespace P2Project.Core.Dtos.Accounts;

public class UserDto
{
    public Guid Id { get; init; }
    public string FirstName { get; init; } = string.Empty;
    public string SecondName { get; init; } = string.Empty;
    public string? LastName { get; init; } = string.Empty;
    public string UserName { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public IEnumerable<SocialNetworkDto> SocialNetworks { get; set; } = default!;
    public IEnumerable<PhotoDto> Photos { get; set; } = default!;
    public AdminAccountDto? AdminAccount { get; init; }
    public VolunteerAccountDto? VolunteerAccount { get; init; }
    public ParticipantAccountDto? ParticipantAccount { get; init; } = null!;
}