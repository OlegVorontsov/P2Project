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
            var volunteer = CreateVolunteer();
            var pet = CreatePet();

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
            int petsCount = 5;
            var volunteerWithPets = CreateVolunteerWithPets(petsCount);
            var petToAdd = CreatePet();

            // act
            var result = volunteerWithPets.AddPet(petToAdd);

            // assert
            var petNotFirstAddedResult = volunteerWithPets.GetPetById(petToAdd.Id);

            result.IsSuccess.Should().BeTrue();
            petNotFirstAddedResult.IsSuccess.Should().BeTrue();
            petNotFirstAddedResult.Value.Id.Should().Be(petToAdd.Id);
            petNotFirstAddedResult.Value.Position.Should().Be(
                         Position.Create(petsCount + 1));
        }

        [Fact]
        public void Move_Pet_Should_Not_Move_If_Pet_Already_At_NewPosition()
        {
            // arrange
            int petsCount = 5;
            var volunteerWithPets = CreateVolunteerWithPets(petsCount);

            var secondPosition = Position.Create(2).Value;

            var firstPet = volunteerWithPets.Pets[0];
            var secondPet = volunteerWithPets.Pets[1];
            var thirdPet = volunteerWithPets.Pets[2];
            var fourthPet = volunteerWithPets.Pets[3];
            var fifthPet = volunteerWithPets.Pets[4];

            // act
            var moveResult = volunteerWithPets.MovePet(
                secondPet, secondPosition);

            // assert
            moveResult.IsSuccess.Should().BeTrue();

            firstPet.Position.Should().Be(Position.Create(1).Value);
            secondPet.Position.Should().Be(Position.Create(2).Value);
            thirdPet.Position.Should().Be(Position.Create(3).Value);
            fourthPet.Position.Should().Be(Position.Create(4).Value);
            fifthPet.Position.Should().Be(Position.Create(5).Value);
        }

        [Fact]
        public void Move_Pet_Should_Move_Other_Pets_Forward_When_NewPosition_Is_Lower()
        {
            // arrange
            int petsCount = 5;
            var volunteerWithPets = CreateVolunteerWithPets(petsCount);

            var secondPosition = Position.Create(2).Value;

            var firstPet = volunteerWithPets.Pets[0];
            var secondPet = volunteerWithPets.Pets[1];
            var thirdPet = volunteerWithPets.Pets[2];
            var fourthPet = volunteerWithPets.Pets[3];
            var fifthPet = volunteerWithPets.Pets[4];

            // act
            var moveResult = volunteerWithPets.MovePet(
                fourthPet, secondPosition);

            // assert
            moveResult.IsSuccess.Should().BeTrue();

            firstPet.Position.Should().Be(Position.Create(1).Value);
            secondPet.Position.Should().Be(Position.Create(3).Value);
            thirdPet.Position.Should().Be(Position.Create(4).Value);
            fourthPet.Position.Should().Be(Position.Create(2).Value);
            fifthPet.Position.Should().Be(Position.Create(5).Value);
        }

        [Fact] 
        public void Move_Pet_Should_Move_Other_Pets_Back_When_NewPosition_Is_Greater()
        {
            // arrange
            int petsCount = 5;
            var volunteerWithPets = CreateVolunteerWithPets(petsCount);

            var fourthPosition = Position.Create(4).Value;

            var firstPet = volunteerWithPets.Pets[0];
            var secondPet = volunteerWithPets.Pets[1];
            var thirdPet = volunteerWithPets.Pets[2];
            var fourthPet = volunteerWithPets.Pets[3];
            var fifthPet = volunteerWithPets.Pets[4];

            // act
            var moveResult = volunteerWithPets.MovePet(
                secondPet, fourthPosition);

            // assert
            moveResult.IsSuccess.Should().BeTrue();

            firstPet.Position.Should().Be(Position.Create(1).Value);
            secondPet.Position.Should().Be(Position.Create(4).Value);
            thirdPet.Position.Should().Be(Position.Create(2).Value);
            fourthPet.Position.Should().Be(Position.Create(3).Value);
            fifthPet.Position.Should().Be(Position.Create(5).Value);
        }

        [Fact]
        public void Move_Pet_Should_Move_Other_Pets_Forward_When_NewPosition_Is_First()
        {
            // arrange
            int petsCount = 5;
            var volunteerWithPets = CreateVolunteerWithPets(petsCount);

            var firstPosition = Position.Create(1).Value;

            var firstPet = volunteerWithPets.Pets[0];
            var secondPet = volunteerWithPets.Pets[1];
            var thirdPet = volunteerWithPets.Pets[2];
            var fourthPet = volunteerWithPets.Pets[3];
            var fifthPet = volunteerWithPets.Pets[4];

            // act
            var moveResult = volunteerWithPets.MovePet(
                fifthPet, firstPosition);

            // assert
            moveResult.IsSuccess.Should().BeTrue();

            firstPet.Position.Should().Be(Position.Create(2).Value);
            secondPet.Position.Should().Be(Position.Create(3).Value);
            thirdPet.Position.Should().Be(Position.Create(4).Value);
            fourthPet.Position.Should().Be(Position.Create(5).Value);
            fifthPet.Position.Should().Be(Position.Create(1).Value);
        }

        [Fact]
        public void Move_Pet_Should_Move_Other_Pets_Back_When_NewPosition_Is_Last()
        {
            // arrange
            int petsCount = 5;
            var volunteerWithPets = CreateVolunteerWithPets(petsCount);

            var fifthPosition = Position.Create(5).Value;

            var firstPet = volunteerWithPets.Pets[0];
            var secondPet = volunteerWithPets.Pets[1];
            var thirdPet = volunteerWithPets.Pets[2];
            var fourthPet = volunteerWithPets.Pets[3];
            var fifthPet = volunteerWithPets.Pets[4];

            // act
            var moveResult = volunteerWithPets.MovePet(
                firstPet, fifthPosition);

            // assert
            moveResult.IsSuccess.Should().BeTrue();

            firstPet.Position.Should().Be(Position.Create(5).Value);
            secondPet.Position.Should().Be(Position.Create(1).Value);
            thirdPet.Position.Should().Be(Position.Create(2).Value);
            fourthPet.Position.Should().Be(Position.Create(3).Value);
            fifthPet.Position.Should().Be(Position.Create(4).Value);
        }

        private Volunteer CreateVolunteer()
        {
            return new Volunteer(
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
        }

        private Pet CreatePet()
        {
            return new Pet(
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
        }

        private Volunteer CreateVolunteerWithPets(int petsCount)
        {
            var volunteer = CreateVolunteer();

            var pets = Enumerable.Range(1, petsCount).Select(_ =>
                CreatePet());

            foreach (var pet in pets)
                volunteer.AddPet(pet);

            return volunteer;
        }
    }
}