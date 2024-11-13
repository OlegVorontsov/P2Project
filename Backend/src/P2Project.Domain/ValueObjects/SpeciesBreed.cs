using CSharpFunctionalExtensions;
using P2Project.Domain.IDs;

namespace P2Project.Domain.ValueObjects
{
    public class SpeciesBreed : ValueObject
    {
        public SpeciesBreed(SpeciesId speciesId, Guid breedId)
        {
            SpeciesId = speciesId;
            BreedId = breedId;
        }
        public SpeciesId SpeciesId { get; } = default!;
        public Guid BreedId { get; } = default!;
        protected override IEnumerable<object> GetEqualityComponents()
        {
            throw new NotImplementedException();
        }
    }
}
