using FluentAssertions;
using P2Project.Domain.PetManagment.ValueObjects.Pets;
using P2Project.UnitTestsFabrics;

namespace P2Project.Domain.UnitTests
{
    public class VolunteerTests
    {
        [Fact]
        public void Add_first_pet_to_volunteer()
        {
            // arrange
            var volunteer = VolunteerFabric.CreateVolunteer();
            var pet = PetFabric.CreatePet();

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
        public void Add_pet_to_volunteer_with_pets()
        {
            // arrange
            int petsCount = 5;
            var volunteerWithPets = VolunteerFabric.CreateVolunteerWithPets(
                petsCount);
            var petToAdd = PetFabric.CreatePet();

            // act
            var result = volunteerWithPets.AddPet(petToAdd);

            // assert
            var petNotFirstAddedResult = volunteerWithPets.GetPetById(petToAdd.Id);

            result.IsSuccess.Should().BeTrue();
            petNotFirstAddedResult.IsSuccess.Should().BeTrue();
            petNotFirstAddedResult.Value.Id.Should().Be(petToAdd.Id);
            petNotFirstAddedResult.Value.Position.Value.Should().Be(
                         Position.Create(petsCount + 1).Value);
        }

        [Fact]
        public void Move_pet_to_it_position()
        {
            // arrange
            int petsCount = 5;
            var volunteerWithPets = VolunteerFabric.CreateVolunteerWithPets(
                petsCount);

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
        public void Move_pet_to_lower_position()
        {
            // arrange
            int petsCount = 5;
            var volunteerWithPets = VolunteerFabric.CreateVolunteerWithPets(
                petsCount);

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
        public void Move_pet_to_greater_position()
        {
            // arrange
            int petsCount = 5;
            var volunteerWithPets = VolunteerFabric.CreateVolunteerWithPets(
                petsCount);

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
        public void Move_last_pet_to_first_position()
        {
            // arrange
            int petsCount = 5;
            var volunteerWithPets = VolunteerFabric.CreateVolunteerWithPets(
                petsCount);

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
        public void Move_first_pet_to_last_position()
        {
            // arrange
            int petsCount = 5;
            var volunteerWithPets = VolunteerFabric.CreateVolunteerWithPets(
                petsCount);

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

        [Fact]
        public void Soft_delete_volunteer()
        {
            // arrange
            int petsCount = 5;
            var volunteerWithPets = VolunteerFabric.CreateVolunteerWithPets(
                petsCount);
            
            // act
            volunteerWithPets.SoftDelete();
            
            // assert
            volunteerWithPets.IsDeleted.Should().BeTrue();
            foreach (var pet in volunteerWithPets.Pets)
            {
                pet.IsDeleted.Should().BeTrue();
            }
        }
        
        [Fact]
        public void Restore_volunteer()
        {
            // arrange
            int petsCount = 5;
            var volunteerWithPets = VolunteerFabric.CreateVolunteerWithPets(
                petsCount);
            volunteerWithPets.SoftDelete();
            
            // act
            volunteerWithPets.Restore();
            
            // assert
            volunteerWithPets.IsDeleted.Should().BeFalse();
            foreach (var pet in volunteerWithPets.Pets)
            {
                pet.IsDeleted.Should().BeFalse();
            }
        }
    }
}