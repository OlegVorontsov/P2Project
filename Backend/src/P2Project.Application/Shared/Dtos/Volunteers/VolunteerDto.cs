using P2Project.Application.Shared.Dtos.Common;
using P2Project.Application.Shared.Dtos.Pets;
using P2Project.Domain.PetManagment.ValueObjects.Pets;

namespace P2Project.Application.Shared.Dtos.Volunteers
{
    public class VolunteerDto
    {
        public Guid Id { get; init; }
        public string FirstName { get; init; } = string.Empty;
        public string SecondName { get; init; } = string.Empty;
        public string LastName { get; init; } = string.Empty;
        public int Age { get; init; }
        public int Grade { get; init; }
        public string Gender { get; init; } = string.Empty;
        public string Email { get; init; } = string.Empty;
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
        public IEnumerable<SocialNetworkDto> SocialNetworks { get; init; } = default!;
        public IEnumerable<AssistanceDetailDto> AssistanceDetails { get; init; } = default!;
        public IEnumerable<PetDto> Pets { get; init; } = default!;
    }
}
