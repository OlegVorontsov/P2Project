using CSharpFunctionalExtensions;

namespace P2Project.Domain.IDs
{
    public class PetId : ValueObject
    {
        private PetId(Guid value)
        {
            Value = value;
        }
        public Guid Value { get; }
        public static PetId NewPetId => new(Guid.NewGuid());
        public static PetId EmptyPetId => new(Guid.Empty);

        protected override IEnumerable<object> GetEqualityComponents()
        {
            throw new NotImplementedException();
        }
    }
}
