using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using P2Project.Core.Interfaces.Commands;
using P2Project.IntegrationTests.Extensions;
using P2Project.IntegrationTests.Factories;
using P2Project.Species.Application.Commands.Create;

namespace P2Project.IntegrationTests.Handlers.Species.Create;

public class CreateTest : SpeciesFactory
{
    private readonly ICommandHandler<Guid, CreateCommand> _sut;

    public CreateTest(IntegrationTestsFactory factory) : base(factory)
    {
        var scope = factory.Services.CreateScope();
        _sut = scope.ServiceProvider
            .GetRequiredService<ICommandHandler<Guid, CreateCommand>>();
    }
    
    [Fact]
    public async Task CreateSpeciesInDatabase()
    {
        // Arrange
        var command = _fixture.FakeCreateSpeciesCommand();

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().Be(true);
        result.Value.Should().NotBeEmpty();

        var species = _speciesWriteDbContext.Species.ToList();
        species.Should().NotBeEmpty();
        species.Should().HaveCount(1);

        var breeds = _speciesReadDbContext.Species
            .Include(s => s.Breeds).ToList().First().Breeds.ToList();
        breeds.Should().NotBeEmpty();
    }
}