using P2Project.Application.Shared.Dtos;

namespace P2Project.Application.Volunteers.UpdateSocialNetworks
{
    public record UpdateSocialNetworksRequest(
        Guid VolunteerId,
        UpdateSocialNetworksDto SocialNetworksDto);

    public record UpdateSocialNetworksDto(
        IEnumerable<SocialNetworkDto> SocialNetworks);
}
