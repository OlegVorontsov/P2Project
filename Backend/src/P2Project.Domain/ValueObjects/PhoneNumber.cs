using CSharpFunctionalExtensions;
using P2Project.Domain.Shared;

namespace P2Project.Domain.ValueObjects
{
    public class PhoneNumber : ValueObject
    {
        private PhoneNumber() { }
        private PhoneNumber(string value, bool? isMain)
        {
            Value = value;
            IsMain = isMain;
        }
        public string Value { get; } = default!;
        public bool? IsMain { get; } = default!;
        public static Result<PhoneNumber, Error> Create(string value, bool? isMain)
        {
            if (string.IsNullOrWhiteSpace(value))
                return Errors.General.ValueIsInvalid(nameof(PhoneNumber));

            var newPhoneNumber = new PhoneNumber(value, isMain);

            return newPhoneNumber;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}
