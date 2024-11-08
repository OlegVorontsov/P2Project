using P2Project.Domain.IDs;
using P2Project.Domain.Shared;
using P2Project.Domain.ValueObjects;

namespace P2Project.Domain.Models
{
    public enum Gender
    {
        Male,
        Female
    }
    public sealed class Volunteer : Shared.Entity<VolunteerId>
    {
        private Volunteer(VolunteerId id) : base(id) { }
        private readonly List<Pet> _pets = [];

        private Volunteer(VolunteerId volunteerId,
                          FullName fullName,
                          int age,
                          Gender gender,
                          Email email,
                          Description description,
                          DateTime registeredDate,
                          VolunteerPhoneNumbers phoneNumbers,
                          VolunteerSocialNetworks? socialNetworks,
                          VolunteerAssistanceDetails? assistanceDetails) : base(volunteerId)
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
        public VolunteerId VolunteerId { get; private set; }
        public FullName FullName { get; private set; }
        public int Age { get; private set; }
        public Gender Gender { get; private set; }
        public Email Email { get; private set; } = default!;
        public Description Description { get; private set; } = default!;
        public DateTime RegisteredDate { get; private set; }
        public double YearsOfExperience => GetYearsOfExperience();
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
        public static Result<Volunteer> Create(VolunteerId volunteerId,
                                               FullName fullName,
                                               int age,
                                               Gender gender,
                                               Email email,
                                               Description description,
                                               DateTime registeredDate,
                                               VolunteerPhoneNumbers phoneNumbers,
                                               VolunteerSocialNetworks? socialNetworks,
                                               VolunteerAssistanceDetails? assistanceDetails)
        {
            var volunteer = new Volunteer(volunteerId, fullName, age, gender,
                                          email, description, registeredDate,
                                          phoneNumbers, socialNetworks, assistanceDetails);

            return volunteer;
        }
        private double GetYearsOfExperience()
        {
            var timeSpan = DateTime.Now - RegisteredDate;
            return timeSpan.TotalDays / 365.25;
        }
        // test ID-B-4.2
    }
}
