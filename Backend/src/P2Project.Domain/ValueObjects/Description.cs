using CSharpFunctionalExtensions;

namespace P2Project.Domain.ValueObjects
{
    public class Description : ValueObject
    {
        private Description(string? value)
        {
            Value = value;
        }
        public string? Value { get; } = default!;
        public static Result<Description,string> Create(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return "Description can't be empty";

            var newDescription = new Description(value);

            return newDescription;
        }

        protected override IEnumerable<IComparable> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}
