using P2Project.Domain.Shared;

namespace P2Project.Domain.ValueObjects
{
    public class SocialNetwork
    {
        private SocialNetwork(string name, string link)
        {
            Name = name;
            Link = link;
        }
        public string Name { get; set; }
        public string Link { get; set; }
        public static Result<SocialNetwork> Create(string name, string link)
        {
            if (string.IsNullOrEmpty(name))
                return "Name can't be empty";
            if (string.IsNullOrEmpty(link))
                return "Link can't be empty";

            var newSocialNetwork = new SocialNetwork(name, link);

            return newSocialNetwork;
        }
    }
}
