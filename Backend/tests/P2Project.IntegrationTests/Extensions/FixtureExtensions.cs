using AutoFixture;
using P2Project.Application.Shared.Dtos.Common;
using P2Project.Application.Shared.Dtos.Volunteers;
using P2Project.Application.Volunteers.Commands.Create;
using P2Project.Domain.PetManagment;

namespace P2Project.IntegrationTests.Extensions;

public static class FixtureExtensions
{
    public static CreateCommand FakeCreateVolunteerCommand(this IFixture fixture)
    {
        var phoneNumber = new PhoneNumberDto("+7 123 456-78-11", false);
        var volunteerInfo = new VolunteerInfoDto(33, 5);
        
        return fixture.Build<CreateCommand>()
            .With(v => v.VolunteerInfo, volunteerInfo)
            .With(v => v.Gender, Gender.Male.ToString())
            .With(v => v.Email, "email@mail.com")
            .With(v => v.PhoneNumbers, [phoneNumber])
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