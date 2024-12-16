using CSharpFunctionalExtensions;
using P2Project.Domain.Shared;
using System.Text.RegularExpressions;
using P2Project.Domain.Shared.Errors;

namespace P2Project.Domain.PetManagment.ValueObjects
{
    public class Email : ValueObject
    {
        private const string EMAIL_CHECK_REGEX = @"^[A-Z0-9._%+-]+@[A-Z0-9-]+\.[A-Z]{2,4}$";
        private Email(string value)
        {
            Value = value;
        }
        public string Value { get; } = default!;
        public static Result<Email, Error> Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return Errors.General.ValueIsInvalid(nameof(Email));

            if (Regex.IsMatch(value, EMAIL_CHECK_REGEX, RegexOptions.IgnoreCase) == false)
                return Errors.General.ValueIsInvalid(nameof(Email));

            var newEmail = new Email(value);

            return newEmail;
        }

        protected override IEnumerable<IComparable> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}
