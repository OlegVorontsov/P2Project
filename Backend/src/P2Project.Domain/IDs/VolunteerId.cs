using CSharpFunctionalExtensions;

namespace P2Project.Domain.IDs
{
    public class VolunteerId : ValueObject
    {
        private VolunteerId(Guid value)
        {
            Value = value;
        }
        public Guid Value { get; }
        public static VolunteerId NewVolunteerId => new(Guid.NewGuid());
        public static VolunteerId EmptyVolunteerId => new(Guid.Empty);
        protected override IEnumerable<object> GetEqualityComponents()
        {
            throw new NotImplementedException();
        }
    }
}
