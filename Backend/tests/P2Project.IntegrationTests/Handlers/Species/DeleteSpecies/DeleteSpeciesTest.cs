using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using P2Project.Core.Interfaces.Commands;
using P2Project.IntegrationTests.Extensions;
using P2Project.IntegrationTests.Factories;
using P2Project.Species.Application.Commands.DeleteSpeciesById;

namespace P2Project.IntegrationTests.Handlers.Species.DeleteSpecies;

public class DeleteSpeciesTest : IntegrationTestBase
{
    private readonly ICommandHandler<Guid, DeleteSpeciesByIdCommand> _sut;

    public DeleteSpeciesTest(IntegrationTestsFactory factory) : base(factory)
    {
        var scope = factory.Services.CreateScope();
        _sut = scope.ServiceProvider
            .GetRequiredService<ICommandHandler<Guid, DeleteSpeciesByIdCommand>>();
    }
    
    [Fact]
    public async Task DeleteSpecies()
    {
        // Arrange
        var species = await SeedSpecies();
        var command = _fixture.FakeDeleteSpeciesCommand(species.Id);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().Be(true);
        result.Value.Should().NotBeEmpty();

        var speciesExist = _speciesWriteDbContext.Species.ToList();
        speciesExist.Should().BeEmpty();
        speciesExist.Should().HaveCount(0);
    }
}