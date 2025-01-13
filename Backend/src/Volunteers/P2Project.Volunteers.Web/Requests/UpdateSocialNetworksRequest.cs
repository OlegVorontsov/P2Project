using P2Project.Core.Dtos.Volunteers;
using P2Project.Volunteers.Application.Commands.UpdateSocialNetworks;

namespace P2Project.Volunteers.Web.Requests;

public record UpdateSocialNetworksRequest(
    IEnumerable<SocialNetworkDto> SocialNetworks)
{
    public UpdateSocialNetworksCommand ToCommand(Guid volunteerId) =>
        new(volunteerId, SocialNetworks);
}
