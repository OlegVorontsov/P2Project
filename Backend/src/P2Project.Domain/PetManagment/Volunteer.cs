using P2Project.Domain.PetManagment.Entities;
using P2Project.Domain.PetManagment.ValueObjects;
using P2Project.Domain.Shared;
using P2Project.Domain.Shared.IDs;

namespace P2Project.Domain.PetManagment
{
    public enum Gender
    {
        Male,
        Female
    }
    public sealed class Volunteer : Entity<VolunteerId>, ISoftDeletable
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

        public void Deleted()
        {
            if(_isDeleted == false)
                _isDeleted = true;
        }

        public void Restored()
        {
            if (_isDeleted == true)
                _isDeleted = false;
        }

        private double GetYearsOfExperience()
        {
            var timeSpan = DateTime.Now - RegisteredDate;
            return timeSpan.TotalDays / 365.25;
        }
    }
}
