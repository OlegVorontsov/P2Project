using FluentAssertions;
using P2Project.Domain.PetManagment;
using P2Project.Domain.PetManagment.Entities;
using P2Project.Domain.PetManagment.ValueObjects;
using P2Project.Domain.Shared;
using P2Project.Domain.Shared.IDs;
using Color = P2Project.Domain.PetManagment.ValueObjects.Color;

namespace VolunteerTests
{
    public class VolunteerTests
    {
        Random random = new Random();
        [Fact]
        public void Add_Pet_PetFirst_Return_Success_Result()
        {
            // arrange
            var volunteer = new Volunteer(
                VolunteerId.New(),
                FullName.Create(
                    "FirstName",
                    "SecondName",
                    "LastName").Value,
                random.Next(Constants.MIN_AGE, Constants.MAX_AGE),
                Gender.Male,
                Email.Create("test@domain.com").Value,
                Description.Create("description").Value,
                DateTime.Now,
                new VolunteerPhoneNumbers(new List<PhoneNumber>()),
                new VolunteerSocialNetworks(new List<SocialNetwork>()),
                new VolunteerAssistanceDetails(new List<AssistanceDetail>()));

            var pet = new Pet(
                PetId.New(),
                NickName.Create("NickName").Value,
                new SpeciesBreed(
                    SpeciesId.Create(Guid.NewGuid()), Guid.NewGuid()),
                Description.Create("description").Value,
                Color.Create("Color").Value,
                HealthInfo.Create("HealthInfo").Value,
                Address.Create(
                    "Region",
                    "City",
                    "Street",
                    "House",
                    "Floor",
                    "Apartment").Value,
                random.Next(
                    Constants.MIN_WEIGHT_HEIGHT, Constants.MAX_WEIGHT_HEIGHT),
                random.Next(
                    Constants.MIN_WEIGHT_HEIGHT, Constants.MAX_WEIGHT_HEIGHT),
                PhoneNumber.Create("+7 123 456-78-90", false).Value,
                false,
                false,
                DateOnly.FromDateTime(DateTime.Today),
                AssistanceStatus.Create("AssistanceStatus").Value,
                new PetAssistanceDetails(new List<AssistanceDetail>()),
                DateOnly.FromDateTime(DateTime.Today));

            // act
            var result = volunteer.AddPet(pet);

            // assert
            var petAddedResult = volunteer.GetPetById(pet.Id);

            result.IsSuccess.Should().BeTrue();
            petAddedResult.IsSuccess.Should().BeTrue();
            petAddedResult.Value.Id.Should().Be(pet.Id);
            petAddedResult.Value.Position.Should().Be(Position.First());
        }

        [Fact]
        public void Add_Pet_Pets_Return_Success_Result()
        {
            // arrange
            const int petsCount = 5;

            var volunteer = new Volunteer(
                VolunteerId.New(),
                FullName.Create(
                    "FirstName",
                    "SecondName",
                    "LastName").Value,
                random.Next(Constants.MIN_AGE, Constants.MAX_AGE),
                Gender.Male,
                Email.Create("test@domain.com").Value,
                Description.Create("description").Value,
                DateTime.Now,
                new VolunteerPhoneNumbers(new List<PhoneNumber>()),
                new VolunteerSocialNetworks(new List<SocialNetwork>()),
                new VolunteerAssistanceDetails(new List<AssistanceDetail>()));

            var pets = Enumerable.Range(1, petsCount).Select(_ =>
                new Pet(
                    PetId.New(),
                    NickName.Create("NickName").Value,
                    new SpeciesBreed(
                        SpeciesId.Create(Guid.NewGuid()), Guid.NewGuid()),
                    Description.Create("description").Value,
                    Color.Create("Color").Value,
                    HealthInfo.Create("HealthInfo").Value,
                    Address.Create(
                        "Region",
                        "City",
                        "Street",
                        "House",
                        "Floor",
                        "Apartment").Value,
                    random.Next(
                        Constants.MIN_WEIGHT_HEIGHT, Constants.MAX_WEIGHT_HEIGHT),
                    random.Next(
                        Constants.MIN_WEIGHT_HEIGHT, Constants.MAX_WEIGHT_HEIGHT),
                    PhoneNumber.Create("+7 123 456-78-90", false).Value,
                    false,
                    false,
                    DateOnly.FromDateTime(DateTime.Today),
                    AssistanceStatus.Create("AssistanceStatus").Value,
                    new PetAssistanceDetails(new List<AssistanceDetail>()),
                    DateOnly.FromDateTime(DateTime.Today)));

            foreach (var pet in pets)
                volunteer.AddPet(pet);

            var petToAdd = new Pet(
                PetId.New(),
                NickName.Create("NickName").Value,
                new SpeciesBreed(
                    SpeciesId.Create(Guid.NewGuid()), Guid.NewGuid()),
                Description.Create("description").Value,
                Color.Create("Color").Value,
                HealthInfo.Create("HealthInfo").Value,
                Address.Create(
                    "Region",
                    "City",
                    "Street",
                    "House",
                    "Floor",
                    "Apartment").Value,
                random.Next(
                    Constants.MIN_WEIGHT_HEIGHT, Constants.MAX_WEIGHT_HEIGHT),
                random.Next(
                    Constants.MIN_WEIGHT_HEIGHT, Constants.MAX_WEIGHT_HEIGHT),
                PhoneNumber.Create("+7 123 456-78-90", false).Value,
                false,
                false,
                DateOnly.FromDateTime(DateTime.Today),
                AssistanceStatus.Create("AssistanceStatus").Value,
                new PetAssistanceDetails(new List<AssistanceDetail>()),
                DateOnly.FromDateTime(DateTime.Today));

            // act
            var result = volunteer.AddPet(petToAdd);

            // assert
            var petNotFirstAddedResult = volunteer.GetPetById(petToAdd.Id);

            result.IsSuccess.Should().BeTrue();
            petNotFirstAddedResult.IsSuccess.Should().BeTrue();
            petNotFirstAddedResult.Value.Id.Should().Be(petToAdd.Id);
            petNotFirstAddedResult.Value.Position.Should().Be(
                         Position.Create(petsCount + 1).Value);
        }
    }
}