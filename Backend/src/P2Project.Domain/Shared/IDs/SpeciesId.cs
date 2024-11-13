namespace P2Project.Domain.Shared.IDs
{
    public record SpeciesId
    {
        private SpeciesId(Guid value)
        {
            Value = value;
        }
        public Guid Value { get; }
        public static SpeciesId NewSpeciesId() => new(Guid.NewGuid());
        public static SpeciesId EmptySpeciesId() => new(Guid.Empty);
        public static SpeciesId CreateSpeciesId(Guid id) => new(id);
        public static implicit operator Guid(SpeciesId speciesId)
        {
            ArgumentNullException.ThrowIfNull(speciesId);
            return speciesId.Value;
        }
    }
}
