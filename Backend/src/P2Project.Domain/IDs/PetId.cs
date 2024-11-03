
namespace P2Project.Domain.IDs
{
    public class PetId
    {
        private PetId(Guid value)
        {
            Value = value;
        }
        public Guid Value { get; }
        public static PetId NewPetId => new(Guid.NewGuid());
        public static PetId EmptyPetId => new(Guid.Empty);
        public static PetId CreatePetId(Guid id) => new(id);
    }
}
