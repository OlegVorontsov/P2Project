using CSharpFunctionalExtensions;
using P2Project.Domain.Shared.Errors;
using P2Project.Domain.Shared.IDs;
using P2Project.Domain.SpeciesManagment.ValueObjects;

namespace P2Project.Domain.SpeciesManagment.Entities
{
    public class Breed : Shared.BaseClasses.Entity<BreedId>
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
