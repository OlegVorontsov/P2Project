using CSharpFunctionalExtensions;

namespace P2Project.Domain.ValueObjects
{
    public class HealthInfo : ValueObject
    {
        private HealthInfo(string? value)
        {
            Value = value;
        }
        public string? Value { get; } = default!;
        public static Result<HealthInfo, string> Create(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return "HealthInfo can't be empty";

            var newHealthInfo = new HealthInfo(value);

            return newHealthInfo;
        }
        protected override IEnumerable<IComparable> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}
