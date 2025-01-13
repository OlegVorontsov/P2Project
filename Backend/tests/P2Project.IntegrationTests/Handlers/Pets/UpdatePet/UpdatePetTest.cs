using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using P2Project.Core.Interfaces.Commands;
using P2Project.IntegrationTests.Extensions;
using P2Project.IntegrationTests.Factories;
using P2Project.Volunteers.Application.Commands.UpdatePet;

namespace P2Project.IntegrationTests.Handlers.Pets.UpdatePet;

public class UpdatePetTest : IntegrationTestBase
{
    private readonly ICommandHandler<Guid, UpdatePetCommand> _sut;

    public UpdatePetTest(IntegrationTestsFactory factory) : base(factory)
    {
        var scope = factory.Services.CreateScope();
        _sut = scope.ServiceProvider
            .GetRequiredService<ICommandHandler<Guid, UpdatePetCommand>>();
    }
    
    [Fact]
    public async Task UpdatePet()
    {
        // Arrange
        var volunteerId = await SeedVolunteer();
        var species = await SeedSpecies();
        var petId = await SeedPet(volunteerId);
        
        var command = _fixture.FakeUpdatePetCommand(
            volunteerId,
            petId,
            species.Id,
            species.Breeds.First().Id);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().Be(true);
        result.Value.Should().NotBeEmpty();

        var volunteers = _volunteersWriteDbContext.Volunteers.ToList();
        volunteers.Should().NotBeEmpty();
        volunteers.Should().HaveCount(1);
        
        var pets = _volunteersReadDbContext.Pets.ToList();
        pets.Should().NotBeEmpty();
        pets.Should().HaveCount(1);
    }
}