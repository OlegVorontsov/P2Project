using P2Project.Core;
using P2Project.SharedKernel;
using P2Project.SharedKernel.IDs;
using P2Project.SharedKernel.ValueObjects;
using P2Project.Volunteers.Domain;

namespace P2Project.UnitTestsFabrics
{
    public static class VolunteerFabric
    {
        static Random random = new Random();
        public static Volunteer CreateVolunteer()
        {
            return new Volunteer(
                VolunteerId.New(),
                VolunteerInfo.Create(
                    random.Next(Constants.MIN_AGE, Constants.MAX_AGE),
                    random.Next(Constants.MIN_GRADE, Constants.MAX_GRADE)).Value,
                Gender.Male,
                Description.Create("description").Value,
                new List<PhoneNumber>());
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
