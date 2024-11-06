
namespace P2Project.Domain.IDs
{
    public record PetId
    {
        private PetId(Guid value)
        {
            Value = value;
        }
        public Guid Value { get; }
        public static PetId NewPetId => new(Guid.NewGuid());
        public static PetId EmptyPetId => new(Guid.Empty);
        public static PetId CreatePetId(Guid id) => new(id);
        public static implicit operator Guid(PetId petId)
        {
            ArgumentNullException.ThrowIfNull(petId);
            return petId.Value;
        }
    }
}
