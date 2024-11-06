using P2Project.Domain.Shared;

namespace P2Project.Domain.ValueObjects
{
    public record NotEmptyString
    {
        public NotEmptyString(string value)
        {
            Value = value;
        }
        public const int MAX_LENGTH = 100;
        public string Value { get; }
        public static Result<NotEmptyString>Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value) || value.Length > MAX_LENGTH)
                return "Value isn't valid";
            return new NotEmptyString(value);
        }
    }
}
