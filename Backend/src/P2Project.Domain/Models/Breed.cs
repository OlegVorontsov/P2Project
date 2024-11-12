using CSharpFunctionalExtensions;
using P2Project.Domain.ValueObjects;

namespace P2Project.Domain.Models
{
    public class Breed
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
    }
}
