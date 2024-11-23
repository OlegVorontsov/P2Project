using P2Project.Domain.PetManagment.ValueObjects;
using P2Project.Domain.Shared;
using P2Project.Domain.Shared.IDs;

namespace P2Project.Domain.PetManagment.Entities
{
    public class Pet : Entity<PetId>, ISoftDeletable
    {
        // ef core
        private Pet(PetId id) : base(id) { }

        private readonly List<PetPhoto> _petPhotos = [];
        private bool _isDeleted = false;
        // ef navigation
        public Volunteer Volunteer { get; private set; } = null!;
        public Pet(
               PetId id,
               NickName nickName,
               SpeciesBreed speciesBreed,
               Description description,
               Color color,
               HealthInfo healthInfo,
               Address address,
               double weight,
               double height,
               PhoneNumber ownerPhoneNumber,
               bool isCastrated,
               bool isVaccinated,
               DateOnly dateOfBirth,
               AssistanceStatus assistanceStatus,
               PetAssistanceDetails? assistanceDetails,
               DateOnly createdAt) : base(id)
        {
            NickName = nickName;
            SpeciesBreed = speciesBreed;
            Description = description;
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
        public NickName NickName { get; private set; } = default!;
        public SpeciesBreed SpeciesBreed { get; private set; } = default!;
        public Description Description { get; private set; } = default!;
        public Color Color { get; private set; } = default!;
        public HealthInfo HealthInfo { get; private set; } = default!;
        public Address Address { get; private set; }
        public double Weight { get; private set; }
        public double Height { get; private set; }
        public PhoneNumber OwnerPhoneNumber { get; private set; } = default!;
        public bool IsCastrated { get; private set; }
        public bool IsVaccinated { get; private set; }
        public DateOnly DateOfBirth { get; private set; }
        public AssistanceStatus AssistanceStatus { get; private set; }
        public PetAssistanceDetails? AssistanceDetails { get; private set; } = default!;
        public DateOnly CreatedAt { get; private set; }
        public IReadOnlyList<PetPhoto> PetPhotos => _petPhotos;
        public void AddPetPhoto(PetPhoto petPhoto) => _petPhotos.Add(petPhoto);

        public void Deleted()
        {
            if (_isDeleted) return;

            _isDeleted = true;
            foreach (var petPhoto in _petPhotos)
                petPhoto.Deleted();
        }
        public void Restored()
        {
            if (!_isDeleted) return;

            _isDeleted = false;
            foreach (var petPhoto in _petPhotos)
                petPhoto.Restored();
        }
    }
}
