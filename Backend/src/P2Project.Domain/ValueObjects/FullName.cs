using P2Project.Domain.Shared;

namespace P2Project.Domain.ValueObjects
{
    public record FullName
    {
        private FullName(string firstName, string secondName, string? lastName)
        {
            FirstName = firstName;
            SecondName = secondName;
            LastName = lastName;
        }
        public string FirstName { get; } = default!;
        public string SecondName { get; } = default!;
        public string? LastName { get; }
        public static Result<FullName>Create(string firstName,
                                             string secondName,
                                             string? lastName)
        {
            if (string.IsNullOrWhiteSpace(firstName))
                return "FirstName can't be empty";
            if (string.IsNullOrWhiteSpace(secondName))
                return "SecondName can't be empty";
            var newFullName = new FullName(firstName, secondName, lastName);
            return newFullName;
        }
    }
}
