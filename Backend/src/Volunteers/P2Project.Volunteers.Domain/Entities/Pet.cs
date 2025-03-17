using CSharpFunctionalExtensions;
using FilesService.Core.Models;
using P2Project.SharedKernel.BaseClasses;
using P2Project.SharedKernel.Errors;
using P2Project.SharedKernel.IDs;
using P2Project.SharedKernel.ValueObjects;
using P2Project.Volunteers.Domain.ValueObjects.Pets;
using Result = CSharpFunctionalExtensions.Result;

namespace P2Project.Volunteers.Domain.Entities
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
               MediaFile avatar = null,
               List<MediaFile>? photos = null) : base(id)
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
            Avatar = avatar;
            Photos = photos ?? [];
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
        public MediaFile? Avatar { get; private set; }
        public IReadOnlyList<MediaFile> Photos { get; private set; } = null!;
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

        public void UpdatePhotos(List<MediaFile> photos) =>
            Photos = photos;

        internal Result<string[], Error> DeleteAllPhotos()
        {
            var photosToDelete = Photos.Select(p => p.FileName);
            Photos = [];
            return photosToDelete.ToArray();
        }

        public void Update(
            NickName nickName,
            SpeciesBreed speciesBreed,
            Description description,
            Color color,
            HealthInfo healthInfo,
            Address address,
            PhoneNumber phoneNumber,
            DateOnly birthDate,
            AssistanceStatus assistanceStatus,
            List<AssistanceDetail>? assistanceDetails)
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
            if (assistanceDetails != null)
                AssistanceDetails = assistanceDetails;
        }

        public void ChangeStatus(AssistanceStatus newStatus)
        {
            AssistanceStatus = newStatus;
        }

        public Result<string, Error> ChangeMainPhoto(
            MediaFile media)
        {
            var photoExist = Photos.FirstOrDefault(p =>
                p.FileName == media.FileName && p.BucketName == media.BucketName);
            if (photoExist is null)
                return Errors.General.NotFound();

            if (photoExist.IsMain == true)
                return Errors.General.Failure(media.FileName);

            var newPhotos = new List<MediaFile>();
            foreach (var photo in Photos)
            {
                if (photo.FileName != media.FileName)
                {
                    if (photo.BucketName != null && photo.Key != null && photo.FileName != null)
                        newPhotos.Add(MediaFile.Create(
                            photo.BucketName,
                            photo.Key.ToString(),
                            photo.FileName, false).Value);
                }
            }
            
            newPhotos.Add(media);
            
            Photos = newPhotos;

            return media.FileName;
        }
        
        public void SetAvatar(MediaFile avatar)
        {
            Avatar = avatar;
        }
    }
}
