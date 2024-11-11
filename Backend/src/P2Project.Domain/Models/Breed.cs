using CSharpFunctionalExtensions;
using P2Project.Domain.ValueObjects;

namespace P2Project.Domain.Models
{
    public class Breed
    {
        public Breed(Guid breedId,
                      Name name)
        {
            BreedId = breedId;
            Name = name;
        }
        public Guid BreedId { get; private set; }
        public Name Name { get; private set; }
    }
}
