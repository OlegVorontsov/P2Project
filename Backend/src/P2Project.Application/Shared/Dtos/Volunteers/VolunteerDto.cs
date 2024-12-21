
using P2Project.Application.Shared.Dtos.Pets;

namespace P2Project.Application.Shared.Dtos.Volunteers
{
    public class VolunteerDto
    {
        public Guid Id { get; init; }
        /*public string FullName { get; init; } = string.Empty;
        public int Age { get; init; }
        public string Gender { get; init; } = string.Empty;
        public string Email { get; init; } = string.Empty;
        public string Description { get; init; } = string.Empty;
        public string RegisteredAt { get; private set; } = string.Empty;
        public double YearsOfExperience { get; init; }
        public string PhoneNumber { get; init; } = string.Empty;
        public string SocialNetwork { get; init; } = string.Empty;
        public string AssistanceDetail { get; init; } = string.Empty;*/
        public PetDto[] Pets { get; init; } = [];
    }
}
