using CSharpFunctionalExtensions;
using P2Project.SharedKernel.Errors;

namespace P2Project.Accounts.Domain.User.ValueObjects
{
    public record SocialNetwork
    {
        public const string DB_COLUMN_SOCIAL_NETWORK = "social_network";

        private SocialNetwork(string name, string link)
        {
            Name = name;
            Link = link;
        }
        public string Name { get; } = default!;
        public string Link { get; } = default!;
        public static Result<SocialNetwork, Error> Create(
                                            string name,
                                            string link)
        {
            if (string.IsNullOrWhiteSpace(name))
                return Errors.General.ValueIsInvalid(nameof(Name));
            if (string.IsNullOrWhiteSpace(link))
                return Errors.General.ValueIsInvalid(nameof(Link));

            return new SocialNetwork(name, link);
        }
    }
}
