using CSharpFunctionalExtensions;
using P2Project.Domain.PetManagment.ValueObjects;
using P2Project.Domain.Shared;
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
        public Result<Guid, Error> AddBreeds(
            IReadOnlyCollection<Breed> breeds)
        {
            var result =
                  from inb in _breeds
                  join exb in breeds
                    on inb.Name equals exb.Name
                  select inb.Name;

            if (result != null && result.Any())
            {
                return Errors.General.ValueIsInvalid(string.Join(", ", result));
            }
            _breeds.AddRange(breeds);
            return Id.Value;
        }
    }
}
