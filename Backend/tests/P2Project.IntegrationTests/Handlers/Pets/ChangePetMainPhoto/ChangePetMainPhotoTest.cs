using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using P2Project.Core.Interfaces.Commands;
using P2Project.IntegrationTests.Extensions;
using P2Project.IntegrationTests.Factories;
using P2Project.Volunteers.Application.Commands.ChangePetMainPhoto;

namespace P2Project.IntegrationTests.Handlers.Pets.ChangePetMainPhoto;

public class ChangePetMainPhotoTest : FileProviderFactory
{
    private readonly ICommandHandler<Guid, ChangePetMainPhotoCommand> _sut;

    public ChangePetMainPhotoTest(IntegrationTestsFactory factory) : base(factory)
    {
        var scope = factory.Services.CreateScope();
        _sut = scope.ServiceProvider
            .GetRequiredService<ICommandHandler<Guid, ChangePetMainPhotoCommand>>();
    }
    
    [Fact]
    public async Task ChangePetMainPhoto()
    {
        // Arrange
        var (volunteerId, species) = await SeedVolunteerAndSpecies();
        var petId = await SeedPetWithPhoto(volunteerId);
        
        var command = _fixture.FakeChangePetMainPhotoCommand(
            volunteerId, petId);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().Be(true);

        var volunteers = _volunteersWriteDbContext.Volunteers.ToList();
        volunteers.Should().NotBeEmpty();
        volunteers.Should().HaveCount(1);
        
        var pets = _volunteersReadDbContext.Pets.ToList();
        pets.Should().NotBeEmpty();
        pets.Should().HaveCount(1);
        pets.First().Photos.Count().Should().Be(1);
    }
}