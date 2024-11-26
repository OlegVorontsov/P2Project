using P2Project.Domain.PetManagment.ValueObjects;
using P2Project.Domain.Shared;
using P2Project.Domain.Shared.IDs;

namespace P2Project.Domain.PetManagment.Entities
{
    public class Pet : Entity<PetId>
    {
        // ef core
        private Pet(PetId id) : base(id) { }

        private bool _isDeleted = false;

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
               DateOnly createdAt,
               IEnumerable<PetPhoto>? photos = null) : base(id)
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

            Photos = photos == null
                ? new PetPhotoList(Enumerable.Empty<PetPhoto>())
                : new PetPhotoList(photos);
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
        public PetPhotoList Photos { get; private set; }

        public void SoftDelete()
        {
            if (_isDeleted)
                return;

            _isDeleted = true;
        }
    }
}
