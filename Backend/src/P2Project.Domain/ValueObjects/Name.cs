using CSharpFunctionalExtensions;

namespace P2Project.Domain.ValueObjects
{
    public class Name : ValueObject
    {
        private Name(string value)
        {
            Value = value;
        }
        public string Value { get; } = default!;
        public static Result<Name, string> Create(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return "Name can't be empty";
            var newname = new Name(name);
            return newname;
        }
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}
