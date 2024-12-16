using CSharpFunctionalExtensions;
using P2Project.Domain.Shared;
using P2Project.Domain.Shared.Errors;

namespace P2Project.Domain.PetManagment.ValueObjects
{
    public class NickName : ValueObject
    {
        private NickName(string value)
        {
            Value = value;
        }
        public string Value { get; } = default!;
        public static Result<NickName, Error> Create(string nickName)
        {
            if (string.IsNullOrWhiteSpace(nickName))
                return Errors.General.ValueIsInvalid(nameof(NickName));
            var newNickName = new NickName(nickName);
            return newNickName;
        }
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}
