using AutoFixture;
using P2Project.Application.Shared.Dtos.Common;
using P2Project.Application.Shared.Dtos.Pets;
using P2Project.Application.Shared.Dtos.Volunteers;
using P2Project.Application.Volunteers.Commands.AddPet;
using P2Project.Application.Volunteers.Commands.Create;
using P2Project.Application.Volunteers.Commands.DeletePetPhotos;
using P2Project.Application.Volunteers.Commands.UpdateAssistanceDetails;
using P2Project.Application.Volunteers.Commands.UpdateMainInfo;
using P2Project.Application.Volunteers.Commands.UpdatePhoneNumbers;
using P2Project.Application.Volunteers.Commands.UpdateSocialNetworks;
using P2Project.Domain.PetManagment;

namespace P2Project.IntegrationTests.Extensions;

public static class FixtureExtensions
{
    public static CreateCommand FakeCreateVolunteerCommand(this IFixture fixture)
    {
        var phoneNumber = new PhoneNumberDto("+7 123 456-78-11", false);
        var volunteerInfo = new VolunteerInfoDto(33, 5);
        
        return fixture.Build<CreateCommand>()
            .With(c => c.VolunteerInfo, volunteerInfo)
            .With(c => c.Gender, Gender.Male.ToString())
            .With(c => c.Email, "email@mail.com")
            .With(c => c.PhoneNumbers, [phoneNumber])
            .Create();
    }
    
    public static UpdateMainInfoCommand FakeUpdateMainInfoCommand(
        this IFixture fixture,
        Guid VolunteerId)
    {
        var volunteerInfo = new VolunteerInfoDto(33, 5);
        
        return fixture.Build<UpdateMainInfoCommand>()
            .With(c => c.VolunteerId, VolunteerId)
            .With(c => c.VolunteerInfo, volunteerInfo)
            .With(c => c.Gender, Gender.Male.ToString())
            .Create();
    }
    
    public static UpdatePhoneNumbersCommand FakeUpdatePhoneNumbersCommand(
        this IFixture fixture,
        Guid VolunteerId)
    {
        var phoneNumber = new PhoneNumberDto("+7 123 456-78-11", false);
        
        return fixture.Build<UpdatePhoneNumbersCommand>()
            .With(c => c.VolunteerId, VolunteerId)
            .With(c => c.PhoneNumbers, [phoneNumber, phoneNumber])
            .Create();
    }
    
    public static UpdateSocialNetworksCommand FakeUpdateSocialNetworksCommand(
        this IFixture fixture,
        Guid VolunteerId)
    {
        var socialNetwork = new SocialNetworkDto("test_name", "test_link");
        
        return fixture.Build<UpdateSocialNetworksCommand>()
            .With(c => c.VolunteerId, VolunteerId)
            .With(c => c.SocialNetworks, [socialNetwork, socialNetwork])
            .Create();
    }
    
    public static UpdateAssistanceDetailsCommand FakeUpdateAssistanceDetailsCommand(
        this IFixture fixture,
        Guid VolunteerId)
    {
        var assistanceDetail = new AssistanceDetailDto(
            "test_name", "test_description", "test_account_number");
        
        return fixture.Build<UpdateAssistanceDetailsCommand>()
            .With(c => c.VolunteerId, VolunteerId)
            .With(c => c.AssistanceDetails, [assistanceDetail, assistanceDetail])
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
        this IFixture fixture,
        Guid VolunteerId,
        Guid PetId)
    {
        return fixture.Build<DeletePetPhotosCommand>()
            .With(c => c.VolunteerId, VolunteerId)
            .With(c => c.PetId, PetId)
            .Create();
    }
    
    /*public static AddPetPhotosCommand CreateAddPetPhotosCommand(
        this Fixture fixture,
        Guid VolunteerId,
        Guid PetId)
    {
        var stream = new MemoryStream();
        var fileName = "test.jpg";
        var uploadFileDto = new UploadFileDto(stream, fileName);

        return fixture.Build<AddPetPhotosCommand>()
            .With(c => c.VolunteerId, VolunteerId)
            .With(c => c.PetId, PetId)
            .With(c => c.Files, [uploadFileDto, uploadFileDto])
            .Create();
    }*/
}