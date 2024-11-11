using CSharpFunctionalExtensions;
using P2Project.Domain.Shared;

namespace P2Project.Domain.ValueObjects
{
    public class Color : ValueObject
    {
        private Color(string value)
        {
            Value = value;
        }
        public string Value { get; } = default!;
        public static Result<Color, Error> Create(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return Errors.General.ValueIsInvalid(nameof(Color));

            var newColor = new Color(value);

            return newColor;
        }
        protected override IEnumerable<IComparable> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}
