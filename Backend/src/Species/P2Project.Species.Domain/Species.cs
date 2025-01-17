using CSharpFunctionalExtensions;
using P2Project.SharedKernel.Errors;
using P2Project.SharedKernel.IDs;
using P2Project.Species.Domain.Entities;
using P2Project.Species.Domain.ValueObjects;

namespace P2Project.Species.Domain
{
    public class Species : SharedKernel.BaseClasses.Entity<SpeciesId>
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
                return Errors.BreedError.AlreadyExist();
            }
            _breeds.AddRange(breeds);
            return Id.Value;
        }
        
        public UnitResult<Error> DeleteBreed(Breed breed)
        {
            var deleted = _breeds.Remove(breed);
            if (deleted == false)
                return Errors.SpeciesError.BreedDelete(breed.Id);

            return Result.Success<Error>();
        }
    }
}
