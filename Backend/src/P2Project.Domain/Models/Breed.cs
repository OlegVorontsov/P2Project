using CSharpFunctionalExtensions;
using P2Project.Domain.ValueObjects;

namespace P2Project.Domain.Models
{
    public class Breed
    {
        private Breed(Guid id) { }
        private Breed(Guid id,
                      Name name)
        {
            Id = id;
            Name = name;
        }
        public Guid Id { get; private set; }
        public Name Name { get; private set; }
        public static Result<Breed> Create(Guid id,
                                           Name name)
        {
            var breed = new Breed(id, name);
            return breed;
        }
    }
}
