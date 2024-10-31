
using P2Project.Domain.IDs;

namespace P2Project.Domain.Models
{
    public class Volunteer : Shared.Entity<VolunteerId>
    {
        private Volunteer(VolunteerId id) : base(id) { }

        public VolunteerId VolunteerId { get; private set; }
        public string FirstName { get; private set; } = default!;
        public string SecondName { get; private set; } = default!;
        public string LastName { get; private set; } = default!;
        public string Email { get; private set; } = default!;
        public string Description { get; private set; } = default!;



    }
}
