using CSharpFunctionalExtensions;
using P2Project.Domain.Shared;
using P2Project.Domain.Shared.Errors;
using P2Project.Domain.SpeciesManagment.ValueObjects;

namespace P2Project.Domain.SpeciesManagment.Entities
{
    public class Breed : ValueObject
    {
        private Breed(Guid id) { }
        public Breed(Name name)
        {
            Id = Guid.Empty;
            Name = name;
        }
        public Guid Id { get; private set; }
        public Name Name { get; private set; }
        public static Result<Breed, Error> Create(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return Errors.General.ValueIsInvalid(nameof(Breed));
            var newName = Name.Create(name);

            var newBreed = new Breed(newName.Value);
            return newBreed;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Name;
        }
    }
}
