using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using P2Project.Core.Interfaces.Commands;
using P2Project.IntegrationTests.Extensions;
using P2Project.IntegrationTests.Factories;
using P2Project.Volunteers.Application.Commands.DeletePetPhotos;

namespace P2Project.IntegrationTests.Handlers.Pets.DeletePetPhotos;

public class DeletePetPhotosTest : FileProviderFactory
{
    private readonly ICommandHandler<DeletePetPhotosCommand> _sut;
    
    public DeletePetPhotosTest(IntegrationTestsFactory factory) : base(factory)
    {
        var scope = factory.Services.CreateScope();
        _sut = scope.ServiceProvider
            .GetRequiredService<ICommandHandler<DeletePetPhotosCommand>>();
    }
    
    [Fact]
    public async Task DeletePetPhotos()
    {
        // Arrange
        var volunteerId = await SeedVolunteer();
        var species = await SeedSpecies();
        var petId = await SeedPet(volunteerId);
        
        var command = _fixture.FakeDeletePetPhotosCommand(
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
        pets.First().Photos.Count().Should().Be(0);
    }
}