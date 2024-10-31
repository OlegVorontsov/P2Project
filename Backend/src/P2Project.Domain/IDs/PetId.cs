using CSharpFunctionalExtensions;

namespace P2Project.Domain.IDs
{
    public class PetId : ValueObject
    {
        public Guid Value { get; }

        private PetId(Guid value)
        {
            Value = value;
        }

        public static PetId NewPetId => new(Guid.NewGuid());
        public static PetId EmptyPetId => new(Guid.Empty);

        protected override IEnumerable<object> GetEqualityComponents()
        {
            throw new NotImplementedException();
        }
    }
}
