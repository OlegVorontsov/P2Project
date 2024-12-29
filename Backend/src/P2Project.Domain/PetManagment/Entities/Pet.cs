using CSharpFunctionalExtensions;
using P2Project.Domain.PetManagment.ValueObjects;
using P2Project.Domain.PetManagment.ValueObjects.Common;
using P2Project.Domain.PetManagment.ValueObjects.Pets;
using P2Project.Domain.Shared;
using P2Project.Domain.Shared.BaseClasses;
using P2Project.Domain.Shared.Errors;
using P2Project.Domain.Shared.IDs;
using Result = CSharpFunctionalExtensions.Result;

namespace P2Project.Domain.PetManagment.Entities
{
    public class Pet : SoftDeletableEntity<PetId>
    {
        public const string DB_TABLE_PETS = "pets";
        public const string DB_COLUMN_BIRTH_DATE = "birth_date";
        public const string DB_COLUMN_CREATED_AT = "created_at";
        public const string DB_COLUMN_ASSISTANCE_DETAILS = "assistance_details";
        public const string DB_COLUMN_PHOTOS = "photos";
        // ef core
        private Pet(PetId id) : base(id) { }

        public Pet(
               PetId id,
               NickName nickName,
               SpeciesBreed speciesBreed,
               Description description,
               Color color,
               HealthInfo healthInfo,
               Address address,
               PhoneNumber phoneNumber,
               DateOnly birthDate,
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
            PhoneNumber = phoneNumber;
            BirthDate = birthDate;
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
        public PhoneNumber PhoneNumber { get; private set; } = default!;
        public DateOnly BirthDate { get; private set; }
        public AssistanceStatus AssistanceStatus { get; private set; }
        public DateOnly CreatedAt { get; private set; }
        public IReadOnlyList<AssistanceDetail> AssistanceDetails { get; private set; } = null!;
        public IReadOnlyList<PetPhoto> Photos { get; private set; } = null!;
        public Position Position { get; private set; }
        public VolunteerId VolunteerId { get; private set; } = null!;

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
    }
}
