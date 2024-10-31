using CSharpFunctionalExtensions;

namespace P2Project.Domain.IDs
{
    public class VolunteerId : ValueObject
    {
        public Guid Value { get; }
        public VolunteerId(Guid value)
        {
            Value = value;
        }
        public static VolunteerId NewVolunteerId => new(Guid.NewGuid());
        public static VolunteerId EmptyVolunteerId => new(Guid.Empty);
        protected override IEnumerable<object> GetEqualityComponents()
        {
            throw new NotImplementedException();
        }
    }
}
