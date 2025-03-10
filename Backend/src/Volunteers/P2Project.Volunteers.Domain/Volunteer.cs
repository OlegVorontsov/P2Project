using CSharpFunctionalExtensions;
using FilesService.Core.Models;
using P2Project.SharedKernel;
using P2Project.SharedKernel.BaseClasses;
using P2Project.SharedKernel.Errors;
using P2Project.SharedKernel.IDs;
using P2Project.SharedKernel.ValueObjects;
using P2Project.Volunteers.Domain.Entities;
using P2Project.Volunteers.Domain.ValueObjects.Pets;
using Result = CSharpFunctionalExtensions.Result;

namespace P2Project.Volunteers.Domain
{
    public enum Gender
    {
        Male,
        Female
    }
    public class Volunteer : SoftDeletableEntity<VolunteerId>
    {
        public const string DB_TABLE_VOLUNTEERS = "volunteers";
        public const string DB_COLUMN_GENDER = "gender";
        public const string DB_COLUMN_REGISTERED_AT = "registered_at";
        public const string DB_COLUMN_PHONE_NUMBERS = "phone_numbers";
        
        private Volunteer(VolunteerId id) : base(id) { }
        private readonly List<Pet> _pets = [];

        public Volunteer(
                VolunteerId id,
                VolunteerInfo volunteerInfo,
                Gender gender,
                Description description,
                List<PhoneNumber>? phoneNumbers) : base(id)
        {
            VolunteerInfo = volunteerInfo;
            Gender = gender;
            Description = description;
            RegisteredAt = DateTime.UtcNow;
            PhoneNumbers = phoneNumbers ?? new List<PhoneNumber>();
        }
        public VolunteerInfo VolunteerInfo { get; private set; }
        public Gender Gender { get; private set; }
        public Description Description { get; private set; } = default!;
        public DateTime RegisteredAt { get; private set; }
        public IReadOnlyList<Pet> Pets => _pets;
        public int NeedsHelpPets
        {
            get => _pets.Count(p => p.AssistanceStatus.Status == "needshelp");
            private set { }
        }
        public int NeedsFoodPets
        {
            get => _pets.Count(p => p.AssistanceStatus.Status == "needsfood");
            private set { }
        }
        public int OnMedicationPets
        {
            get => _pets.Count(p => p.AssistanceStatus.Status == "onmedication");
            private set { }
        }
        public int LooksForHomePets
        {
            get => _pets.Count(p => p.AssistanceStatus.Status == "looksforhome");
            private set { }
        }
        public int FoundHomePets
        {
            get => _pets.Count(p => p.AssistanceStatus.Status == "foundhome");
            private set { }
        }
        public int UnknownStatusPets{ get; private set; } = default!;
        public IReadOnlyList<PhoneNumber> PhoneNumbers { get; private set; } = [];
        
        public override void SoftDelete()
        {
            base.SoftDelete();

            foreach (var pet in _pets)
            {
                pet.SoftDelete();
            }
        }

        public override void Restore()
        {
            base.Restore();

            foreach (var pet in _pets)
            {
                pet.Restore();
            }
        }

        public void UpdateMainInfo(
                    VolunteerInfo volunteerInfo,
                    Gender gender,
                    Description description)
        {
            VolunteerInfo = volunteerInfo;
            Gender = gender;
            Description = description;
        }

        public void UpdatePhoneNumbers(
            List<PhoneNumber> phoneNumbers) =>
            PhoneNumbers = phoneNumbers;

        public UnitResult<Error> AddPet(Pet pet)
        {
            var positionResult = Position.Create(
                _pets.Count + 1);
            if (positionResult.IsFailure)
                return positionResult.Error;

            pet.SetPosition(positionResult.Value);

            var status = pet.AssistanceStatus.Status.ToLower() switch
            {
                "needshelp" => NeedsHelpPets++,
                "needsfood" => NeedsFoodPets++,
                "onmedication" => OnMedicationPets++,
                "looksforhome" => LooksForHomePets++,
                "foundhome" => FoundHomePets++,
                _ => UnknownStatusPets++,
            };

            _pets.Add(pet);

            return Result.Success<Error>();
        }

        public UnitResult<Error> UpdatePet(
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
            List<AssistanceDetail>? assistanceDetails)
        {
            var petExist = _pets.FirstOrDefault(p => p.Id == id);
            if (petExist is null)
                return Errors.VolunteerError.PetNotFound(Id, id.Value);
            
            petExist.Update(
                nickName,
                speciesBreed,
                description,
                color,
                healthInfo,
                address,
                phoneNumber,
                birthDate,
                assistanceStatus,
                assistanceDetails);
            
            return Result.Success<Error>();
        }

        public UnitResult<Error> ChangePetStatus(
            PetId petId,
            AssistanceStatus newStatus)
        {
            var petExist = _pets.FirstOrDefault(p => p.Id == petId);
            if (petExist is null)
                return Errors.VolunteerError.PetNotFound(Id, petId.Value);

            var currentStatusChange = petExist.AssistanceStatus.Status switch
            {
                "needshelp" => NeedsHelpPets--,
                "needsfood" => NeedsFoodPets--,
                "onmedication" => OnMedicationPets--,
                "looksforhome" => LooksForHomePets--,
                "foundhome" => FoundHomePets--,
                _ => UnknownStatusPets--,
            };
            
            petExist.ChangeStatus(newStatus);
            
            return Result.Success<Error>();
        }

        public Result<string, Error> ChangePetMainPhoto(
            PetId petId,
            MediaFile newMainPhoto)
        {
            var petExist = _pets.FirstOrDefault(p => p.Id == petId);
            if (petExist is null)
                return Errors.VolunteerError.PetNotFound(Id, petId.Value);
            
            var changeResult = petExist.ChangeMainPhoto(newMainPhoto);
            if(changeResult.IsFailure)
                return changeResult.Error;
            
            return changeResult.Value;
        }
        
        public Result<string[], Error> DeletePetPhotos(PetId petId)
        {
            var pet = Pets.FirstOrDefault(p => p.Id == petId);
            if (pet is null)
                return Errors.VolunteerError.PetNotFound(Id, petId);
            
            var deleteResult = pet.DeleteAllPhotos();
            if (deleteResult.IsFailure)
                return deleteResult.Error;
            
            return deleteResult;
        }

        public UnitResult<Error> SoftDeletePet(PetId petId)
        {
            var petResult = GetPetById(petId);
            if (petResult.IsFailure)
                return petResult.Error;

            petResult.Value.SoftDelete();

            return UnitResult.Success<Error>();
        }

        public Result<string[], Error> HardDeletePet(PetId petId)
        {
            var petResult = GetPetById(petId);
            if (petResult.IsFailure)
                return petResult.Error;

            var filesToDelete = petResult.Value.Photos
                .Select(ph => ph.FileName).ToArray();
            
            _pets.Remove(petResult.Value);

            return filesToDelete;
        }

        public void DeleteExpiredPets()
        {
            _pets.RemoveAll(p => p.DeletionDateTime != null &&
                                 DateTime.UtcNow >= p.DeletionDateTime.Value
                                     .AddDays(Constants.LIFETIME_AFTER_SOFT_DELETION));
        }
        
        public Result<Pet, Error> GetPetById(PetId petId)
        {
            var pet = Pets.FirstOrDefault(p => p.Id.Value == petId.Value);
            if (pet is null)
                return Errors.General.NotFound(petId.Value);

            return pet;
        }

        public UnitResult<Error> MovePet(
            Pet pet, Position newPosition)
        {
            var currentPosition = pet.Position;

            if (currentPosition == newPosition || _pets.Count == 1)
                return Result.Success<Error>();

            var positionToSet = ChangePositionIfOutOfRange(newPosition);
            if (positionToSet.IsFailure)
                return positionToSet.Error;

            newPosition = positionToSet.Value;

            var moveResult = MovePetsBetweenPositions(
                newPosition, currentPosition);
            if (moveResult.IsFailure)
                return moveResult.Error;

            pet.SetPosition(newPosition);

            return Result.Success<Error>();
        }

        private Result<Position, Error> ChangePositionIfOutOfRange(
            Position newPosition)
        {
            if (newPosition <= _pets.Count)
                return newPosition;

            var lastPosition = Position.Create(_pets.Count - 1);
            if (lastPosition.IsFailure)
                return lastPosition.Error;

            return lastPosition;
        }

        private UnitResult<Error> MovePetsBetweenPositions(
            Position newPosition, Position currentPosition)
        {
            if (newPosition < currentPosition)
            {
                var petsToMove = _pets.Where(p => p.Position >=
                    newPosition &&
                    p.Position < currentPosition);

                foreach (var petToMove in petsToMove)
                {
                    var moveResult = petToMove.Forward();
                    if (moveResult.IsFailure)
                        return moveResult.Error;
                }
            }
            else if (newPosition > currentPosition)
            {
                var petsToMove = _pets.Where(p => p.Position >
                    currentPosition &&
                    p.Position <= newPosition);

                foreach (var petToMove in petsToMove)
                {
                    var moveResult = petToMove.Back();
                    if (moveResult.IsFailure)
                        return moveResult.Error;
                }
            }
            return Result.Success<Error>();
        }
    }
}
