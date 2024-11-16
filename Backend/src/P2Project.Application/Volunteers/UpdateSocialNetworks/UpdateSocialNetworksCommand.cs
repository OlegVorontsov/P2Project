namespace P2Project.Application.Volunteers.UpdateSocialNetworks
{
    public record UpdateSocialNetworksCommand(
        Guid VolunteerId,
        UpdateSocialNetworksDto SocialNetworksDto);


}
