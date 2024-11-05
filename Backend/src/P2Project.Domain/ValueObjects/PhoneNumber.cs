using P2Project.Domain.Shared;

namespace P2Project.Domain.ValueObjects
{
    public record PhoneNumber
    {
        private PhoneNumber(string value, bool isMain)
        {
            Value = value;
            IsMain = isMain;
        }
        public string Value { get; } = default!;
        public bool IsMain { get; } = default!;
        public static Result<PhoneNumber> Create(string value, bool isMain)
        {
            if (string.IsNullOrWhiteSpace(value))
                return "PhoneNumber can't be empty";

            var newPhoneNumber = new PhoneNumber(value, isMain);

            return newPhoneNumber;
        }
    }
}
