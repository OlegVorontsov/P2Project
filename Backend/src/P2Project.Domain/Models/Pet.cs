using P2Project.Domain.IDs;
using P2Project.Domain.Shared;
using P2Project.Domain.ValueObjects;

namespace P2Project.Domain.Models
{
    public class Pet : Shared.Entity<PetId>
    {
        // ef core
        private Pet(PetId id) : base(id) { }
        
        private readonly List<AssistanceDetail> _assistanceDetails = [];
        private readonly List<PetPhoto> _petPhotos = [];
        
        // ef navigation
        public Volunteer Volunteer { get; private set; } = null!;
        private Pet(PetId petId,
                    string nickName,
                    string species,
                    string description,
                    string breed,
                    string color,
                    string healthInfo,
                    Address address,
                    double weight,
                    double height,
                    string ownerPhoneNumber,
                    bool isCastrated,
                    bool isVaccinated,
                    DateTime dateOfBirth,
                    AssistanceStatus status,
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
            Status = status;
            CreatedAt = createdAt;
        }
        public PetId PetId { get; private set; }
        public string NickName { get; private set; } = default!;
        public string Species { get; private set; } = default!;
        public string Description { get; private set; } = default!;
        public string Breed { get; private set; } = default!;
        public string Color { get; private set; } = default!;
        public string HealthInfo { get; private set; } = default!;
        public Address Address { get; private set; }
        public double Weight { get; private set; }
        public double Height { get; private set; }
        public string OwnerPhoneNumber { get; private set; } = default!;
        public bool IsCastrated { get; private set; }
        public bool IsVaccinated { get; private set; }
        public DateTime DateOfBirth { get; private set; }
        public AssistanceStatus Status { get; private set; }
        public IReadOnlyList<AssistanceDetail> AssistanceDetails => _assistanceDetails;
        public IReadOnlyList<PetPhoto> PetPhotos => _petPhotos;
        public DateTime CreatedAt { get; private set; }
        public static Result<Pet> Create(PetId petId,
                                         string nickName,
                                         string species,
                                         string description,
                                         string breed,
                                         string color,
                                         string healthInfo,
                                         Address address,
                                         double weight,
                                         double height,
                                         string ownerPhoneNumber,
                                         bool isCastrated,
                                         bool isVaccinated,
                                         DateTime dateOfBirth,
                                         AssistanceStatus status,
                                         DateTime createdAt)
        {
            if (string.IsNullOrWhiteSpace(nickName))
            {
                return "Nickame can't be empty";
            }
            if (string.IsNullOrWhiteSpace(species))
            {
                return "Species can't be empty";
            }

            var pet = new Pet(petId, nickName, species, description, breed, color,
                              healthInfo, address, weight, height, ownerPhoneNumber,
                              isCastrated, isVaccinated, dateOfBirth, status, createdAt);

            return pet;
        }
        public void AddAssistanceDetail(AssistanceDetail assistanceDetail)
        {
            _assistanceDetails.Add(assistanceDetail);
        }
    }
}
