using P2Project.Domain.PetManagment.Entities;
using P2Project.Domain.PetManagment.ValueObjects;
using P2Project.Domain.PetManagment.ValueObjects.Common;
using P2Project.Domain.PetManagment.ValueObjects.Pets;
using P2Project.Domain.Shared;
using P2Project.Domain.Shared.IDs;

namespace P2Project.UnitTestsFabrics
{
    public static class PetFabric
    {
        static Random random = new Random();
        public static Pet CreatePet()
        {
            return new Pet(
                PetId.New(),
                NickName.Create("NickName").Value,
                new SpeciesBreed(
                    SpeciesId.Create(Guid.NewGuid()), Guid.NewGuid()),
                Description.Create("description").Value,
                Color.Create("Color").Value,
                HealthInfo.Create(
                    random.Next(
                        Constants.MIN_WEIGHT_HEIGHT, Constants.MAX_WEIGHT_HEIGHT),
                    random.Next(
                        Constants.MIN_WEIGHT_HEIGHT, Constants.MAX_WEIGHT_HEIGHT),
                    true,
                    true,
                    "health_description").Value,
                Address.Create(
                    "Region",
                    "City",
                    "Street",
                    "House",
                    "Floor",
                    "Apartment").Value,
                PhoneNumber.Create("+7 123 456-78-90", false).Value,
                DateOnly.FromDateTime(DateTime.Today),
                AssistanceStatus.Create("AssistanceStatus").Value,
                DateOnly.FromDateTime(DateTime.Today),
                new List<AssistanceDetail>());
        }
        
        public static Pet CreatePet(Guid speciesId, Guid breedId)
        {
            return new Pet(
                PetId.New(),
                NickName.Create("NickName").Value,
                new SpeciesBreed(
                    SpeciesId.Create(speciesId), breedId),
                Description.Create("description").Value,
                Color.Create("Color").Value,
                HealthInfo.Create(
                    random.Next(
                        Constants.MIN_WEIGHT_HEIGHT, Constants.MAX_WEIGHT_HEIGHT),
                    random.Next(
                        Constants.MIN_WEIGHT_HEIGHT, Constants.MAX_WEIGHT_HEIGHT),
                    true,
                    true,
                    "health_description").Value,
                Address.Create(
                    "Region",
                    "City",
                    "Street",
                    "House",
                    "Floor",
                    "Apartment").Value,
                PhoneNumber.Create("+7 123 456-78-90", false).Value,
                DateOnly.FromDateTime(DateTime.Today),
                AssistanceStatus.Create("AssistanceStatus").Value,
                DateOnly.FromDateTime(DateTime.Today),
                new List<AssistanceDetail>());
        }
    }
}
