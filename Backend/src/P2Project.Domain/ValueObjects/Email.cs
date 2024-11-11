using CSharpFunctionalExtensions;
using P2Project.Domain.Shared;

namespace P2Project.Domain.ValueObjects
{
    public class Email : ValueObject
    {
        private Email(string value)
        {
            Value = value;
        }
        public string Value { get; } = default!;
        public static Result<Email, Error> Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
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
