using CSharpFunctionalExtensions;
using P2Project.SharedKernel.Errors;

namespace P2Project.Species.Domain.ValueObjects
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
            var newName = new Name(name);
            return newName;
        }
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}
