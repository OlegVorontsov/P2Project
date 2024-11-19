using CSharpFunctionalExtensions;
using P2Project.Domain.SpeciesManagment.ValueObjects;

namespace P2Project.Domain.SpeciesManagment.Entities
{
    public class Breed : ValueObject
    {
        private Breed(Guid id) { }
        public Breed(Guid id,
                      Name name)
        {
            Id = id;
            Name = name;
        }
        public Guid Id { get; private set; }
        public Name Name { get; private set; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Name;
        }
    }
}
