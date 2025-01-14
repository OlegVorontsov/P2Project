using System.Text.RegularExpressions;
using CSharpFunctionalExtensions;
using P2Project.SharedKernel.Errors;

namespace P2Project.SharedKernel.ValueObjects
{
    public class PhoneNumber : ValueObject
    {
        public const string DB_COLUMN_PHONE_NUMBER = "phone_number";
        public const string DB_COLUMN_IS_MAIN = "is_main";
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
                return Errors.Errors.General.ValueIsInvalid(nameof(PhoneNumber));

            if (Regex.IsMatch(value, PHONE_CHECK_REGEX) == false)
                return Errors.Errors.General.ValueIsInvalid(nameof(PhoneNumber));

            var newPhoneNumber = new PhoneNumber(value, isMain);

            return newPhoneNumber;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}
