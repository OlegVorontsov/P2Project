using CSharpFunctionalExtensions;
using P2Project.Domain.Shared;
using P2Project.Domain.Shared.Errors;

namespace P2Project.Domain.PetManagment.ValueObjects
{
    public class Description : ValueObject
    {
        private Description(string? value)
        {
            Value = value;
        }
        public string? Value { get; } = default!;
        public static Result<Description, Error> Create(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return Errors.General.ValueIsInvalid(nameof(Description));

            var newDescription = new Description(value);

            return newDescription;
        }
        protected override IEnumerable<IComparable> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}
