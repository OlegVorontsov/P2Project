using P2Project.Domain.IDs;
using P2Project.Domain.Shared;
using P2Project.Domain.ValueObjects;

namespace P2Project.Domain.Models
{
    public class Pet : Shared.Entity<PetId>
    {
        // ef core
        private Pet(PetId id) : base(id) { }
        
        private readonly List<PetPhoto> _petPhotos = [];
        
        // ef navigation
        public Volunteer Volunteer { get; private set; } = null!;
        private Pet(PetId petId,
                    NickName nickName,
                    string species,
                    Description description,
                    string breed,
                    Color color,
                    HealthInfo healthInfo,
                    Address address,
                    double weight,
                    double height,
                    PhoneNumber ownerPhoneNumber,
                    bool isCastrated,
                    bool isVaccinated,
                    DateTime dateOfBirth,
                    AssistanceStatus assistanceStatus,
                    PetAssistanceDetails? assistanceDetails,
                    DateTime createdAt) : base(petId)
        {
            NickName = nickName;
            Species = species;
            Description = description;
            Breed = breed;
            Color = color;
            HealthInfo = healthInfo;
            Address = address;
            Weight = weight;
            Height = height;
            OwnerPhoneNumber = ownerPhoneNumber;
            IsCastrated = isCastrated;
            IsVaccinated = isVaccinated;
            DateOfBirth = dateOfBirth;
            AssistanceStatus = assistanceStatus;
            AssistanceDetails = assistanceDetails;
            CreatedAt = createdAt;
        }
        public PetId PetId { get; private set; }
        public NickName NickName { get; private set; } = default!;
        public string Species { get; private set; } = default!;
        public Description Description { get; private set; } = default!;
        public string Breed { get; private set; } = default!;
        public Color Color { get; private set; } = default!;
        public HealthInfo HealthInfo { get; private set; } = default!;
        public Address Address { get; private set; }
        public double Weight { get; private set; }
        public double Height { get; private set; }
        public PhoneNumber OwnerPhoneNumber { get; private set; } = default!;
        public bool IsCastrated { get; private set; }
        public bool IsVaccinated { get; private set; }
        public DateTime DateOfBirth { get; private set; }
        public AssistanceStatus AssistanceStatus { get; private set; }
        public PetAssistanceDetails? AssistanceDetails { get; private set; } = default!;
        public IReadOnlyList<PetPhoto> PetPhotos => _petPhotos;
        public DateTime CreatedAt { get; private set; }
        public static Result<Pet> Create(PetId petId,
                                         NickName nickName,
                                         string species,
                                         Description description,
                                         string breed,
                                         Color color,
                                         HealthInfo healthInfo,
                                         Address address,
                                         double weight,
                                         double height,
                                         PhoneNumber ownerPhoneNumber,
                                         bool isCastrated,
                                         bool isVaccinated,
                                         DateTime dateOfBirth,
                                         AssistanceStatus assistanceStatus,
                                         PetAssistanceDetails assistanceDetails,
                                         DateTime createdAt)
        {
            var pet = new Pet(petId, nickName, species, description, breed, color,
                              healthInfo, address, weight, height, ownerPhoneNumber,
                              isCastrated, isVaccinated, dateOfBirth,
                              assistanceStatus, assistanceDetails, createdAt);
            return pet;
        }
    }
}
