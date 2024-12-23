using CSharpFunctionalExtensions;
using P2Project.Domain.PetManagment.Entities;
using P2Project.Domain.PetManagment.ValueObjects;
using P2Project.Domain.PetManagment.ValueObjects.Common;
using P2Project.Domain.PetManagment.ValueObjects.Pets;
using P2Project.Domain.PetManagment.ValueObjects.Volunteers;
using P2Project.Domain.Shared;
using P2Project.Domain.Shared.Errors;
using P2Project.Domain.Shared.IDs;
using Result = CSharpFunctionalExtensions.Result;

namespace P2Project.Domain.PetManagment
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
        public const string DB_COLUMN_YEARS_OF_EXPERIENCE = "years_of_experience";
        public const string DB_COLUMN_PHONE_NUMBERS = "phone_numbers";
        public const string DB_COLUMN_SOCIAL_NETWORKS = "social_networks";
        public const string DB_COLUMN_ASSISTANCE_DETAILS = "assistance_details";
        private Volunteer(VolunteerId id) : base(id) { }
        private readonly List<Pet> _pets = [];

        public Volunteer(
                VolunteerId id,
                FullName fullName,
                VolunteerInfo volunteerInfo,
                Gender gender,
                Email email,
                Description description,
                DateTime registeredAt,
                List<PhoneNumber>? phoneNumbers,
                List<SocialNetwork>? socialNetworks,
                List<AssistanceDetail>? assistanceDetails) : base(id)
        {
            FullName = fullName;
            VolunteerInfo = volunteerInfo;
            Gender = gender;
            Email = email;
            Description = description;
            RegisteredAt = registeredAt;
            PhoneNumbers = phoneNumbers ?? new List<PhoneNumber>();
            SocialNetworks = socialNetworks ?? new List<SocialNetwork>();
            AssistanceDetails = assistanceDetails ?? new List<AssistanceDetail>();
        }
        public FullName FullName { get; private set; }
        public VolunteerInfo VolunteerInfo { get; private set; }
        public Gender Gender { get; private set; }
        public Email Email { get; private set; } = default!;
        public Description Description { get; private set; } = default!;
        public DateTime RegisteredAt { get; private set; }
        public string YearsOfExperience
        {
            get => GetYearsOfExperience();
            private set { }
        }
        public IReadOnlyList<Pet> Pets => _pets;
        public int NeedsHelpPets =>
            _pets.Count(p => p.AssistanceStatus == AssistanceStatus.NeedsHelp);
        public int NeedsFoodPets =>
            _pets.Count(p => p.AssistanceStatus == AssistanceStatus.NeedsFood);
        public int OnMedicationPets =>
            _pets.Count(p => p.AssistanceStatus == AssistanceStatus.OnMedication);
        public int LooksForHomePets =>
            _pets.Count(p => p.AssistanceStatus == AssistanceStatus.LooksForHome);
        public int FoundHomePets =>
            _pets.Count(p => p.AssistanceStatus == AssistanceStatus.FoundHome);
        public IReadOnlyList<PhoneNumber> PhoneNumbers { get; private set; } = null!;
        public IReadOnlyList<SocialNetwork> SocialNetworks { get; private set; } = null!;
        public IReadOnlyList<AssistanceDetail> AssistanceDetails { get; private set; } = null!;
        private string GetYearsOfExperience()
        {
            var registrationDate = RegisteredAt;
            var currentDate = DateTime.Now;
            
            var years = currentDate.Year - registrationDate.Year;
            var months = currentDate.Month - registrationDate.Month;
            var days = currentDate.Day - registrationDate.Day;
            
            if (days < 0)
            {
                months--;
                days += DateTime.DaysInMonth(currentDate.Year, currentDate.Month);
            }
            
            if (months < 0)
            {
                years--;
                months += 12;
            }
            
            string yearString = years == 1 ? "1 год" : years + " лет";
            string monthString = months == 1 ? "1 месяц" : months + " месяцев";
            string dayString = days == 1 ? "1 день" : days + " дней";

            return $"{yearString} {monthString} {dayString}";
        }
        
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
                    FullName fullName,
                    VolunteerInfo volunteerInfo,
                    Gender gender,
                    Description description)
        {
            FullName = fullName;
            VolunteerInfo = volunteerInfo;
            Gender = gender;
            Description = description;
        }

        public void UpdatePhoneNumbers(
            List<PhoneNumber> phoneNumbers) =>
            PhoneNumbers = phoneNumbers;

        public void UpdateSocialNetworks(
            List<SocialNetwork> socialNetworks) =>
            SocialNetworks = socialNetworks;

        public void UpdateAssistanceDetails(
            List<AssistanceDetail> assistanceDetails) =>
            AssistanceDetails = assistanceDetails;

        public UnitResult<Error> AddPet(Pet pet)
        {
            var positionResult = Position.Create(
                _pets.Count + 1);
            if (positionResult.IsFailure)
                return positionResult.Error;

            pet.SetPosition(positionResult.Value);

            _pets.Add(pet);

            return Result.Success<Error>();
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
