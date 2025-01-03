﻿using CSharpFunctionalExtensions;
using P2Project.Domain.Shared.Errors;

namespace P2Project.Domain.PetManagment.ValueObjects.Volunteers
{
    public class SocialNetwork
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

            var newSocialNetwork = new SocialNetwork(name, link);

            return newSocialNetwork;
        }
    }
}
