using P2Project.Core.Dtos.Volunteers;
using P2Project.Core.Interfaces.Commands;

namespace P2Project.Volunteers.Application.Commands.UpdateSocialNetworks
{
    public record UpdateSocialNetworksCommand(
        Guid VolunteerId,
        IEnumerable<SocialNetworkDto> SocialNetworks) : ICommand;
}
