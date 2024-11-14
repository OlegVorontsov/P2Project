using CSharpFunctionalExtensions;
using P2Project.Domain.Shared;
using System.Text.RegularExpressions;

namespace P2Project.Domain.PetManagment.ValueObjects
{
    public class PhoneNumber : ValueObject
    {
        private const string PHONE_CHECK_REGEX = @"^((8|\+7)[\- ]?)?(\(?\d{3}\)?[\- ]?)?[\d\- ]{7,10}$";
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

            if (Regex.IsMatch(value, PHONE_CHECK_REGEX) == false)
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
