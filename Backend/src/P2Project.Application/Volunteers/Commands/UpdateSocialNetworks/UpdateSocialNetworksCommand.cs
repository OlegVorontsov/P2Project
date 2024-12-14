using P2Project.Application.Shared.Dtos;

namespace P2Project.Application.Volunteers.Commands.UpdateSocialNetworks
{
    public record UpdateSocialNetworksCommand(
        Guid VolunteerId,
        IEnumerable<SocialNetworkDto> SocialNetworks);
}
