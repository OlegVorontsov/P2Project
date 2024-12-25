using CSharpFunctionalExtensions;
using P2Project.Domain.Shared.Errors;

namespace P2Project.Domain.PetManagment.ValueObjects.Volunteers
{
    public record FullName
    {
        public const string DB_COLUMN_FIRST_NAME = "first_name";
        public const string DB_COLUMN_SECOND_NAME = "second_name";
        public const string DB_COLUMN_LAST_NAME = "last_name";
        private FullName(string firstName,
                         string secondName,
                         string? lastName)
        {
            FirstName = firstName;
            SecondName = secondName;
            LastName = lastName;
        }
        public string FirstName { get; } = default!;
        public string SecondName { get; } = default!;
        public string? LastName { get; }
        public static Result<FullName, Error> Create(string firstName,
                                             string secondName,
                                             string? lastName)
        {
            if (string.IsNullOrWhiteSpace(firstName))
                return Errors.General.ValueIsInvalid(nameof(FirstName));
            if (string.IsNullOrWhiteSpace(secondName))
                return Errors.General.ValueIsInvalid(nameof(SecondName));
            var newFullName = new FullName(firstName, secondName, lastName);
            return newFullName;
        }
    }
}
