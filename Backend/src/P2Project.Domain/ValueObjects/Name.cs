using CSharpFunctionalExtensions;
using P2Project.Domain.Shared;

namespace P2Project.Domain.ValueObjects
{
    public class Name : ValueObject
    {
        private Name(string value)
        {
            Value = value;
        }
        public string Value { get; } = default!;
        public static Result<Name, Error> Create(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return Errors.General.ValueIsInvalid(nameof(Name));
            var newname = new Name(name);
            return newname;
        }
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}
