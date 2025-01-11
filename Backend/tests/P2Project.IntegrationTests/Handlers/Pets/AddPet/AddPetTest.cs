using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using P2Project.Application.Interfaces.Commands;
using P2Project.Application.Volunteers.Commands.AddPet;
using P2Project.IntegrationTests.Extensions;
using P2Project.IntegrationTests.Factories;

namespace P2Project.IntegrationTests.Handlers.Pets.AddPet;

public class AddPetTest : IntegrationTestBase
{
    private readonly ICommandHandler<Guid, AddPetCommand> _sut;

    public AddPetTest(IntegrationTestsFactory factory) : base(factory)
    {
        var scope = factory.Services.CreateScope();
        _sut = scope.ServiceProvider
            .GetRequiredService<ICommandHandler<Guid, AddPetCommand>>();
    }
    
    [Fact]
    public async Task AddPet()
    {
        // Arrange
        var volunteerId = await SeedVolunteer();
        var species = await SeedSpecies();
        
        var command = _fixture.FakeAddPetCommand(
            volunteerId,
            species.Id,
            species.Breeds.First().Id);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().Be(true);
        result.Value.Should().NotBeEmpty();

        var volunteers = _writeDbContext.Volunteers.ToList();
        volunteers.Should().NotBeEmpty();
        volunteers.Should().HaveCount(1);
        
        var pets = _volunteersReadDbContext.Pets.ToList();
        pets.Should().NotBeEmpty();
        pets.Should().HaveCount(1);
    }
}