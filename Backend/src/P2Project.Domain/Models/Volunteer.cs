
using CSharpFunctionalExtensions;
using P2Project.Domain.IDs;
using P2Project.Domain.ValueObjects;

namespace P2Project.Domain.Models
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
        private readonly List<SocialNetwork> _socialNetworks = [];
        private readonly List<AssistanceDetail> _assistanceDetails = [];
        private Volunteer(VolunteerId volunteerId,
                          string firstName,
                          string secondName,
                          string lastName,
                          int age,
                          Gender gender,
                          string email,
                          string description,
                          DateTime registeredDate) : base(volunteerId)
        {
            FirstName = firstName;
            SecondName = secondName;
            LastName = lastName;
            Age = age;
            Gender = gender;
            Email = email;
            Description = description;
            RegisteredDate = registeredDate;
        }
        public VolunteerId VolunteerId { get; private set; }
        public string FirstName { get; private set; } = default!;
        public string SecondName { get; private set; } = default!;
        public string LastName { get; private set; } = default!;
        public int Age { get; private set; }
        public Gender Gender { get; private set; }
        public string Email { get; private set; } = default!;
        public string Description { get; private set; } = default!;
        public DateTime RegisteredDate { get; private set; }
        public double YearsOfExperience => GetYearsOfExperience();
        public IReadOnlyList<Pet> Pets => _pets;
        public int NeedsHelpPets =>
            _pets.Count(p => p.Status == AssistanceStatus.NeedsHelp);
        public int NeedsFoodPets =>
            _pets.Count(p => p.Status == AssistanceStatus.NeedsFood);
        public int OnMedicationPets =>
            _pets.Count(p => p.Status == AssistanceStatus.OnMedication);
        public int LooksForHomePets =>
            _pets.Count(p => p.Status == AssistanceStatus.LooksForHome);
        public int FoundHomePets =>
            _pets.Count(p => p.Status == AssistanceStatus.FoundHome);
        public string PhoneNumber {  get; private set; } = default!;
        public IReadOnlyList<SocialNetwork> SocialNetworks => _socialNetworks;
        public IReadOnlyList<AssistanceDetail> AssistanceDetails => _assistanceDetails;
        public static Result<Volunteer> Create(VolunteerId volunteerId,
                                               string firstName,
                                               string secondName,
                                               string lastName,
                                               int age,
                                               Gender gender,
                                               string email,
                                               string description,
                                               DateTime registeredDate)
        {
            if (string.IsNullOrWhiteSpace(firstName))
            {
                return Result.Failure<Volunteer>("FirstName can't be empty");
            }
            if (string.IsNullOrWhiteSpace(secondName))
            {
                return Result.Failure<Volunteer>("SecondName can't be empty");
            }
            if (string.IsNullOrWhiteSpace(lastName))
            {
                return Result.Failure<Volunteer>("LastName can't be empty");
            }

            var volunteer = new Volunteer(volunteerId, firstName, secondName, lastName,
                                          age, gender, email, description, registeredDate);

            return Result.Success(volunteer);
        }
        private double GetYearsOfExperience()
        {
            var timeSpan = DateTime.Now - RegisteredDate;
            return timeSpan.TotalDays / 365.25;
        }
    }
}
