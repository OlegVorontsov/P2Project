using CSharpFunctionalExtensions;
using P2Project.Domain.IDs;
using P2Project.Domain.ValueObjects;

namespace P2Project.Domain.Models
{
    public class Species : Shared.Entity<SpeciesId>
    {
        private Species(SpeciesId id) : base(id) { }
        private readonly List<Breed> _breeds = [];
        public Species(SpeciesId id,
                        Name name) : base(id)
        {
            Name = name;
        }
        public Name Name { get; private set; }
        public IReadOnlyList<Breed> Breeds => _breeds;
    }
}
