using CSharpFunctionalExtensions;
using P2Project.Domain.ValueObjects;

namespace P2Project.Domain.Models
{
    public class Breed
    {
        private Breed(Guid breedId,
                      Name name)
        {
            BreedId = breedId;
            Name = name;
        }
        public Guid BreedId { get; private set; }
        public Name Name { get; private set; }
        public static Result<Breed> Create(Guid breedId,
                                           Name name)
        {
            var breed = new Breed(breedId, name);
            return breed;
        }
    }
}
