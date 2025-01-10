using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using P2Project.Application.Interfaces.Commands;
using P2Project.Application.Species.Commands.AddBreeds;
using P2Project.Application.Species.Commands.DeleteBreedById;
using P2Project.IntegrationTests.Extensions;
using P2Project.IntegrationTests.Factories;

namespace P2Project.IntegrationTests.Handlers.Species.DeleteBreed;

public class DeleteBreedTest : IntegrationTestBase
{
    private readonly ICommandHandler<Guid, DeleteBreedByIdCommand> _sut;

    public DeleteBreedTest(IntegrationTestsFactory factory) : base(factory)
    {
        var scope = factory.Services.CreateScope();
        _sut = scope.ServiceProvider
            .GetRequiredService<ICommandHandler<Guid, DeleteBreedByIdCommand>>();
    }
    
    [Fact]
    public async Task DeleteBreed()
    {
        // Arrange
        var species = await SeedSpecies();
        var breedId = species.Breeds.First().Id;
        var command = _fixture.FakeDeleteBreedCommand(species.Id, breedId);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().Be(true);
        result.Value.Should().NotBeEmpty();

        var speciesExist = _writeDbContext.Species.ToList();
        speciesExist.Should().NotBeEmpty();
        speciesExist.Should().HaveCount(1);
    }
}