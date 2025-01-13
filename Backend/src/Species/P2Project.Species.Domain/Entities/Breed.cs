using P2Project.Core.BaseClasses;
using P2Project.Core.IDs;
using P2Project.Species.Domain.ValueObjects;

namespace P2Project.Species.Domain.Entities
{
    public class Breed : Entity<BreedId>
    {
        private Breed(BreedId id) : base(id) { }
        public Breed(
            BreedId id, Name name) : base(id)
        {
            Name = name;
        }
        public Name Name { get; private set; }
        public SpeciesId SpeciesId { get; private set; } = null!;
    }
}
