using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using P2Project.Application.Interfaces.Commands;
using P2Project.Application.Species.Commands.AddBreeds;
using P2Project.IntegrationTests.Extensions;
using P2Project.IntegrationTests.Factories;

namespace P2Project.IntegrationTests.Handlers.Species.AddBreeds;

public class AddBreedsTest : IntegrationTestBase
{
    private readonly ICommandHandler<Guid, AddBreedsCommand> _sut;

    public AddBreedsTest(IntegrationTestsFactory factory) : base(factory)
    {
        var scope = factory.Services.CreateScope();
        _sut = scope.ServiceProvider
            .GetRequiredService<ICommandHandler<Guid, AddBreedsCommand>>();
    }
    
    [Fact]
    public async Task AddBreeds()
    {
        // Arrange
        var species = await SeedSpecies();
        var command = _fixture.FakeAddBreedsCommand(species.Id);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().Be(true);
        result.Value.Should().NotBeEmpty();

        var speciesExist = _writeDbContext.Species.ToList();
        speciesExist.Should().NotBeEmpty();
        speciesExist.Should().HaveCount(1);

        var breeds = _writeDbContext.Species.Include(s => s.Breeds).ToList().First().Breeds.ToList();
        breeds.Should().NotBeEmpty();
    }
}