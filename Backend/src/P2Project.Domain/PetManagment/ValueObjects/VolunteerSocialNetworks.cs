namespace P2Project.Domain.PetManagment.ValueObjects
{
    public record VolunteerSocialNetworks
    {
        private VolunteerSocialNetworks() { }
        public VolunteerSocialNetworks(IReadOnlyList<SocialNetwork>? socialNetworks)
        {
            SocialNetworks = socialNetworks;
        }
        public IReadOnlyList<SocialNetwork>? SocialNetworks { get; } = default!;
    }
}
