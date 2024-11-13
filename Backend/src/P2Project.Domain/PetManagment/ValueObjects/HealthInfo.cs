using CSharpFunctionalExtensions;
using P2Project.Domain.Shared;

namespace P2Project.Domain.PetManagment.ValueObjects
{
    public class HealthInfo : ValueObject
    {
        private HealthInfo(string? value)
        {
            Value = value;
        }
        public string? Value { get; } = default!;
        public static Result<HealthInfo, Error> Create(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return Errors.General.ValueIsInvalid(nameof(HealthInfo));

            var newHealthInfo = new HealthInfo(value);

            return newHealthInfo;
        }
        protected override IEnumerable<IComparable> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}
