using CSharpFunctionalExtensions;

namespace P2Project.Domain.ValueObjects
{
    public class SocialNetwork : ValueObject
    {
        public string Name { get; set; }
        public string Link { get; set; }
        private SocialNetwork(string name, string link)
        {
            Name = name;
            Link = link;
        }
        public static Result<SocialNetwork> Create(string name, string link)
        {
            if (string.IsNullOrEmpty(name))
                return Result.Failure<SocialNetwork>("Name can't be empty");
            if (string.IsNullOrEmpty(link))
                return Result.Failure<SocialNetwork>("Link can't be empty");

            var newSocialNetwork = new SocialNetwork(name, link);

            return Result.Success(newSocialNetwork);
        }
        protected override IEnumerable<object> GetEqualityComponents()
        {
            throw new NotImplementedException();
        }
    }
}
