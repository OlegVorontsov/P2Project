using CSharpFunctionalExtensions;
using P2Project.Domain.SpeciesManagment.ValueObjects;

namespace P2Project.Domain.SpeciesManagment.Entities
{
    public class Breed : ValueObject
    {
        private Breed(Guid id) { }
        public Breed(Name name)
        {
            Id = Guid.NewGuid();
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
