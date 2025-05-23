using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using P2Project.Core.Interfaces.Commands;
using P2Project.IntegrationTests.Extensions;
using P2Project.IntegrationTests.Factories;
using P2Project.Volunteers.Application.Commands.HardDeletePet;

namespace P2Project.IntegrationTests.Handlers.Pets.HardDeletePet;

public class HardDeletePetTest : VolunteerFactory
{
    private readonly ICommandHandler<Guid, HardDeletePetCommand> _sut;

    public HardDeletePetTest(IntegrationTestsFactory factory) : base(factory)
    {
        var scope = factory.Services.CreateScope();
        _sut = scope.ServiceProvider
            .GetRequiredService<ICommandHandler<Guid, HardDeletePetCommand>>();
    }
    
    [Fact]
    public async Task HardDeletePet()
    {
        // Arrange
        var (volunteerId, species) = await SeedVolunteerAndSpecies();
        var petId = await SeedPet(volunteerId);
        
        var command = _fixture.FakeHardDeletePetCommand(volunteerId, petId);

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
        pets.Should().BeEmpty();
        pets.Should().HaveCount(0);
    }
}