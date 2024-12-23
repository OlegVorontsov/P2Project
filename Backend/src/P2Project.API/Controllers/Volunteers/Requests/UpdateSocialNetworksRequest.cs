using P2Project.Application.Shared.Dtos;
using P2Project.Application.Shared.Dtos.Volunteers;
using P2Project.Application.Volunteers.Commands.UpdateSocialNetworks;

namespace P2Project.API.Controllers.Volunteers.Requests;

public record UpdateSocialNetworksRequest(
    IEnumerable<SocialNetworkDto> SocialNetworks)
{
    public UpdateSocialNetworksCommand ToCommand(Guid volunteerId) =>
        new(volunteerId, SocialNetworks);
}
