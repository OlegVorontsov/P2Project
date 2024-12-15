using CSharpFunctionalExtensions;
using P2Project.Domain.PetManagment.Entities;
using P2Project.Domain.PetManagment.ValueObjects;
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
    public class Volunteer : Shared.Entity<VolunteerId>
    {
        private Volunteer(VolunteerId id) : base(id) { }
        private readonly List<Pet> _pets = [];
        private bool _isDeleted = false;

        public Volunteer(
                VolunteerId id,
                FullName fullName,
                int age,
                Gender gender,
                Email email,
                Description description,
                DateTime registeredDate,
                VolunteerPhoneNumbers phoneNumbers,
                VolunteerSocialNetworks? socialNetworks,
                VolunteerAssistanceDetails? assistanceDetails) : base(id)
        {
            FullName = fullName;
            Age = age;
            Gender = gender;
            Email = email;
            Description = description;
            RegisteredDate = registeredDate;
            PhoneNumbers = phoneNumbers;
            SocialNetworks = socialNetworks;
            AssistanceDetails = assistanceDetails;
        }
        public FullName FullName { get; private set; }
        public int Age { get; private set; }
        public Gender Gender { get; private set; }
        public Email Email { get; private set; } = default!;
        public Description Description { get; private set; } = default!;
        public DateTime RegisteredDate { get; private set; }
        public double YearsOfExperience
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
        public VolunteerPhoneNumbers PhoneNumbers { get; private set; } = default!;
        public VolunteerSocialNetworks? SocialNetworks { get; private set; } = default!;
        public VolunteerAssistanceDetails? AssistanceDetails { get; private set; } = default!;
        private double GetYearsOfExperience()
        {
            var timeSpan = DateTime.Now - RegisteredDate;
            return timeSpan.TotalDays / 365.25;
        }

        public void UpdateMainInfo(
                    FullName fullName,
                    int age,
                    Gender gender,
                    Description description)
        {
            FullName = fullName;
            Age = age;
            Gender = gender;
            Description = description;
        }

        public void UpdatePhoneNumbers(
            VolunteerPhoneNumbers phoneNumbers)
        {
            PhoneNumbers = phoneNumbers;
        }

        public void UpdateSocialNetworks(
            VolunteerSocialNetworks socialNetworks)
        {
            SocialNetworks = socialNetworks;
        }

        public void UpdateAssistanceDetails(
            VolunteerAssistanceDetails assistanceDetails)
        {
            AssistanceDetails = assistanceDetails;
        }

        public void SoftDelete()
        {
            if (_isDeleted)
                return;

            _isDeleted = true;
            foreach (var pet in _pets)
                pet.SoftDelete();
        }

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
