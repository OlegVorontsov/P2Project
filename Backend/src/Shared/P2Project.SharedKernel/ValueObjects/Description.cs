using CSharpFunctionalExtensions;
using P2Project.SharedKernel.Errors;

namespace P2Project.SharedKernel.ValueObjects
{
    public class Description : ValueObject
    {
        public const string DB_COLUMN_DESCRIPTION = "description";
        private Description(string? value)
        {
            Value = value;
        }
        public string? Value { get; } = default!;
        public static Result<Description, Error> Create(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return Errors.Errors.General.ValueIsInvalid(nameof(Description));

            return new Description(value);;
        }
        protected override IEnumerable<IComparable> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}
