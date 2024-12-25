using P2Project.Domain.PetManagment;
using P2Project.Domain.PetManagment.ValueObjects;
using P2Project.Domain.PetManagment.ValueObjects.Common;
using P2Project.Domain.PetManagment.ValueObjects.Volunteers;
using P2Project.Domain.Shared;
using P2Project.Domain.Shared.IDs;

namespace P2Project.UnitTestsFabrics
{
    public static class VolunteerFabric
    {
        static Random random = new Random();
        public static Volunteer CreateVolunteer()
        {
            return new Volunteer(
                VolunteerId.New(),
                FullName.Create(
                    "FirstName",
                    "SecondName",
                    "LastName").Value,
                VolunteerInfo.Create(
                    random.Next(Constants.MIN_AGE, Constants.MAX_AGE),
                    random.Next(Constants.MIN_GRADE, Constants.MAX_GRADE)).Value,
                Gender.Male,
                Email.Create("test@domain.com").Value,
                Description.Create("description").Value,
                new List<PhoneNumber>(),
                new List<SocialNetwork>(),
                new List<AssistanceDetail>());
        }

        public static Volunteer CreateVolunteerWithPets(int petsCount)
        {
            var volunteer = CreateVolunteer();

            var pets = Enumerable.Range(1, petsCount).Select(_ =>
                PetFabric.CreatePet());

            foreach (var pet in pets)
                volunteer.AddPet(pet);

            return volunteer;
        }
    }
}
