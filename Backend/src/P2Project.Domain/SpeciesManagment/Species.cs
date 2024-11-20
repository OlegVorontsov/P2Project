using P2Project.Domain.Shared.IDs;
using P2Project.Domain.SpeciesManagment.Entities;
using P2Project.Domain.SpeciesManagment.ValueObjects;

namespace P2Project.Domain.SpeciesManagment
{
    public class Species : Shared.Entity<SpeciesId>
    {
        private Species(SpeciesId id) : base(id) { }
        private readonly List<Breed> _breeds = [];
        public Species(SpeciesId id,
                        Name name,
                        IReadOnlyList<Breed> breeds) : base(id)
        {
            Name = name;
            _breeds = breeds.ToList();
        }
        public Name Name { get; private set; }
        public IReadOnlyList<Breed> Breeds => _breeds;
    }
}
