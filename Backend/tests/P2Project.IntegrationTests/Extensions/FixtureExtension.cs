using AutoFixture;
using P2Project.Core.Dtos.Common;
using P2Project.Core.Dtos.Pets;
using P2Project.Core.Dtos.Volunteers;
using P2Project.Species.Application.Commands.AddBreeds;
using P2Project.Species.Application.Commands.DeleteBreedById;
using P2Project.Species.Application.Commands.DeleteSpeciesById;
using P2Project.Volunteers.Application.Commands.AddPet;
using P2Project.Volunteers.Application.Commands.ChangePetMainPhoto;
using P2Project.Volunteers.Application.Commands.ChangePetStatus;
using P2Project.Volunteers.Application.Commands.Create;
using P2Project.Volunteers.Application.Commands.DeletePetPhotos;
using P2Project.Volunteers.Application.Commands.HardDeletePet;
using P2Project.Volunteers.Application.Commands.SoftDeletePet;
using P2Project.Volunteers.Application.Commands.UpdateMainInfo;
using P2Project.Volunteers.Application.Commands.UpdatePet;
using P2Project.Volunteers.Application.Commands.UpdatePhoneNumbers;
using P2Project.Volunteers.Domain;

namespace P2Project.IntegrationTests.Extensions;

public static class FixtureExtension
{
    public static CreateCommand FakeCreateVolunteerCommand(this IFixture fixture)
    {
        var phoneNumber = new PhoneNumberDto("+7 123 456-78-11", false);
        var volunteerInfo = new VolunteerInfoDto(33, 5);
        
        return fixture.Build<CreateCommand>()
            .With(c => c.VolunteerInfo, volunteerInfo)
            .With(c => c.Gender, Gender.Male.ToString())
            .With(c => c.PhoneNumbers, [phoneNumber])
            .Create();
    }
    
    public static UpdateMainInfoCommand FakeUpdateMainInfoCommand(
        this IFixture fixture, Guid VolunteerId)
    {
        var volunteerInfo = new VolunteerInfoDto(33, 5);
        
        return fixture.Build<UpdateMainInfoCommand>()
            .With(c => c.VolunteerId, VolunteerId)
            .With(c => c.VolunteerInfo, volunteerInfo)
            .With(c => c.Gender, Gender.Male.ToString())
            .Create();
    }
    
    public static UpdatePhoneNumbersCommand FakeUpdatePhoneNumbersCommand(
        this IFixture fixture, Guid VolunteerId)
    {
        var phoneNumber = new PhoneNumberDto("+7 123 456-78-11", false);
        
        return fixture.Build<UpdatePhoneNumbersCommand>()
            .With(c => c.VolunteerId, VolunteerId)
            .With(c => c.PhoneNumbers, [phoneNumber, phoneNumber])
            .Create();
    }
    
    public static AddPetCommand FakeAddPetCommand(
        this IFixture fixture,
        Guid VolunteerId,
        Guid SpeciesId,
        Guid BreedId)
    {
        var healthInfo = new HealthInfoDto(20, 20, true, true, "test_health_description");
        var ownerPhoneNumber = new PhoneNumberDto("+7 123 456-78-11", false);
        
        return fixture.Build<AddPetCommand>()
            .With(c => c.VolunteerId, VolunteerId)
            .With(c => c.SpeciesId, SpeciesId)
            .With(c => c.BreedId, BreedId)
            .With(c => c.HealthInfo, healthInfo)
            .With(c => c.OwnerPhoneNumber, ownerPhoneNumber)
            .Create();
    }
    
    public static DeletePetPhotosCommand FakeDeletePetPhotosCommand(
        this IFixture fixture, Guid VolunteerId, Guid PetId)
    {
        return fixture.Build<DeletePetPhotosCommand>()
            .With(c => c.VolunteerId, VolunteerId)
            .With(c => c.PetId, PetId)
            .Create();
    }
    
    public static UpdatePetCommand FakeUpdatePetCommand(
        this IFixture fixture,
        Guid VolunteerId,
        Guid PetId,
        Guid SpeciesId,
        Guid BreedId)
    {
        var healthInfo = new HealthInfoDto(20, 20, true, true, "test_health_description");
        var ownerPhoneNumber = new PhoneNumberDto("+7 123 456-78-11", false);
        
        return fixture.Build<UpdatePetCommand>()
            .With(c => c.VolunteerId, VolunteerId)
            .With(c => c.PetId, PetId)
            .With(c => c.SpeciesId, SpeciesId)
            .With(c => c.BreedId, BreedId)
            .With(c => c.HealthInfo, healthInfo)
            .With(c => c.OwnerPhoneNumber, ownerPhoneNumber)
            .Create();
    }
    
    public static ChangePetStatusCommand FakeChangePetStatusCommand(
        this IFixture fixture, Guid VolunteerId, Guid PetId)
    {
        return fixture.Build<ChangePetStatusCommand>()
            .With(c => c.VolunteerId, VolunteerId)
            .With(c => c.PetId, PetId)
            .Create();
    }
    
    public static ChangePetMainPhotoCommand FakeChangePetMainPhotoCommand(
        this IFixture fixture, Guid VolunteerId, Guid PetId)
    {
        var newMainPhotoFileName = "test_file_name.jpg";
        var bucketName = "test_bucket_name";
        
        return fixture.Build<ChangePetMainPhotoCommand>()
            .With(c => c.VolunteerId, VolunteerId)
            .With(c => c.PetId, PetId)
            .With(c => c.BucketName, bucketName)
            .With(c => c.FileName, newMainPhotoFileName)
            .Create();
    }
    
    public static SoftDeletePetCommand FakeSoftDeletePetCommand(
        this IFixture fixture, Guid VolunteerId, Guid PetId)
    {
        return fixture.Build<SoftDeletePetCommand>()
            .With(c => c.VolunteerId, VolunteerId)
            .With(c => c.PetId, PetId)
            .Create();
    }
    
    public static HardDeletePetCommand FakeHardDeletePetCommand(
        this IFixture fixture, Guid VolunteerId, Guid PetId)
    {
        return fixture.Build<HardDeletePetCommand>()
            .With(c => c.VolunteerId, VolunteerId)
            .With(c => c.PetId, PetId)
            .Create();
    }
    
    public static Species.Application.Commands.Create.CreateCommand FakeCreateSpeciesCommand(
        this IFixture fixture)
    {
        return fixture.Build<Species.Application.Commands.Create.CreateCommand>()
            .Create();
    }
    
    public static AddBreedsCommand FakeAddBreedsCommand(
        this IFixture fixture, Guid SpeciesId)
    {
        return fixture.Build<AddBreedsCommand>()
            .With(c => c.SpeciesId, SpeciesId)
            .Create();
    }
    
    public static DeleteSpeciesByIdCommand FakeDeleteSpeciesCommand(
        this IFixture fixture, Guid SpeciesId)
    {
        return fixture.Build<DeleteSpeciesByIdCommand>()
            .With(c => c.Id, SpeciesId)
            .Create();
    }
    
    public static DeleteBreedByIdCommand FakeDeleteBreedCommand(
        this IFixture fixture, Guid SpeciesId, Guid BreedId)
    {
        return fixture.Build<DeleteBreedByIdCommand>()
            .With(c => c.SpeciesId, SpeciesId)
            .With(c => c.BreedId, BreedId)
            .Create();
    }
}