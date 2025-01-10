using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using P2Project.Application.Interfaces.Commands;
using P2Project.Application.Species.Commands.AddBreeds;
using P2Project.Application.Species.Commands.DeleteSpeciesById;
using P2Project.IntegrationTests.Extensions;
using P2Project.IntegrationTests.Factories;

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

        var speciesExist = _writeDbContext.Species.ToList();
        speciesExist.Should().BeEmpty();
        speciesExist.Should().HaveCount(0);
    }
}