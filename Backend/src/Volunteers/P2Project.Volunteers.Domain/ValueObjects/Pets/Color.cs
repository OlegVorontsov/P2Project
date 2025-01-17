using CSharpFunctionalExtensions;
using P2Project.SharedKernel.Errors;

namespace P2Project.Volunteers.Domain.ValueObjects.Pets
{
    public class Color : ValueObject
    {
        public const string DB_COLUMN_COLOR = "color";

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
