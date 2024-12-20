using System.Collections;
using CSharpFunctionalExtensions;
using P2Project.Domain.PetManagment.ValueObjects;
using P2Project.Domain.Shared;
using P2Project.Domain.Shared.Errors;
using P2Project.Domain.Shared.IDs;
using Result = CSharpFunctionalExtensions.Result;

namespace P2Project.Domain.PetManagment.Entities
{
    public class Pet : Shared.Entity<PetId>
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
               DateOnly createdAt,
               List<AssistanceDetail>? assistanceDetails,
               List<PetPhoto>? photos = null) : base(id)
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
            CreatedAt = createdAt;
            AssistanceDetails = assistanceDetails ??
                                new List<AssistanceDetail>([]);
            Photos = photos ??
                     new List<PetPhoto>([]);
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
        public DateOnly CreatedAt { get; private set; }
        public IReadOnlyList<AssistanceDetail> AssistanceDetails { get; private set; } = null!;
        public IReadOnlyList<PetPhoto> Photos { get; private set; } = null!;
        public Position Position { get; private set; }

        public void SetPosition (Position position) =>
            Position = position;

        public UnitResult<Error> Forward()
        {
            var newPosition = Position.Forward();
            if(newPosition.IsFailure)
                return newPosition.Error;

            Position = newPosition.Value;

            return Result.Success<Error>();
        }

        public UnitResult<Error> Back()
        {
            var newPosition = Position.Back();
            if (newPosition.IsFailure)
                return newPosition.Error;

            Position = newPosition.Value;

            return Result.Success<Error>();
        }

        public void UpdatePhotos(List<PetPhoto> photos) =>
            Photos = photos;

        public void SoftDelete()
        {
            if (_isDeleted)
                return;

            _isDeleted = true;
        }
    }
}
