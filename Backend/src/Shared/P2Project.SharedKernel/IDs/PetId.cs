namespace P2Project.SharedKernel.IDs
{
    public record PetId
    {
        private PetId(Guid value)
        {
            Value = value;
        }
        public Guid Value { get; }
        public static PetId New() => new(Guid.NewGuid());
        public static PetId Empty() => new(Guid.Empty);
        public static PetId Create(Guid id) => new PetId(id);
        public static implicit operator Guid(PetId petId)
        {
            ArgumentNullException.ThrowIfNull(petId);
            return petId.Value;
        }
    }
}
