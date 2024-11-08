using CSharpFunctionalExtensions;

namespace P2Project.Domain.ValueObjects
{
    public class Color : ValueObject
    {
        private Color(string value)
        {
            Value = value;
        }
        public string Value { get; } = default!;
        public static Result<Color, string> Create(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return "Color can't be empty";

            var newColor = new Color(value);

            return newColor;
        }
        protected override IEnumerable<IComparable> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}
