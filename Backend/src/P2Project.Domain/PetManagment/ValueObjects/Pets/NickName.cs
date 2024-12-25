using CSharpFunctionalExtensions;
using P2Project.Domain.Shared.Errors;

namespace P2Project.Domain.PetManagment.ValueObjects.Pets
{
    public class NickName : ValueObject
    {
        public const string DB_COLUMN_NICKNAME = "nick_name";
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
