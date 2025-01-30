using P2Project.Core.Dtos.Common;
using P2Project.Core.Dtos.Pets;

namespace P2Project.Core.Dtos.Volunteers
{
    public class VolunteerDto
    {
        public Guid Id { get; init; }
        public int Age { get; init; }
        public int Grade { get; init; }
        public string Gender { get; init; } = string.Empty;
        public string Description { get; init; } = string.Empty;
        public DateTime RegisteredAt { get; init; }
        public string YearsOfExperience { get; init; } = string.Empty;
        public int NeedsHelpPets { get; init; }
        public int NeedsFoodPets { get; init; }
        public int OnMedicationPets { get; init; }
        public int LooksForHomePets { get; init; }
        public int FoundHomePets { get; init; }
        public int UnknownStatusPets { get; init; }
        public bool IsDeleted { get; init; }
        public IEnumerable<PhoneNumberDto> PhoneNumbers { get; init; } = default!;
        public IEnumerable<PetDto> Pets { get; init; } = default!;
    }
}
