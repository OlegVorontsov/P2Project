using CSharpFunctionalExtensions;

namespace P2Project.Domain.ValueObjects
{
    public class NickName : ValueObject
    {
        private NickName(string value)
        {
            Value = value;
        }
        public string Value { get; } = default!;
        public static Result<NickName, string> Create(string nickName)
        {
            if (string.IsNullOrWhiteSpace(nickName))
                return "NickName can't be empty";
            var newNickName = new NickName(nickName);
            return newNickName;
        }
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}
