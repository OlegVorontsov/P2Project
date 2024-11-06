using CSharpFunctionalExtensions;

namespace P2Project.Domain.ValueObjects
{
    public class Email : ValueObject
    {
        private Email(string value)
        {
            Value = value;
        }
        public string Value { get; } = default!;
        public static Result<Email, string> Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return "Email can't be empty";

            var newEmail = new Email(value);

            return newEmail;
        }

        protected override IEnumerable<IComparable> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}
