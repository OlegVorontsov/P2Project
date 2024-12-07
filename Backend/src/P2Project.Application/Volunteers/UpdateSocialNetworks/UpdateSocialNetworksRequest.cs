using P2Project.Application.Shared.Dtos;

namespace P2Project.Application.Volunteers.UpdateSocialNetworks;

public record UpdateSocialNetworksRequest(
    Guid VolunteerId,
    IEnumerable<SocialNetworkDto> SocialNetworks)
{
    public UpdateSocialNetworksCommand ToCommand(Guid volunteerId) =>
        new(volunteerId, SocialNetworks);
}
