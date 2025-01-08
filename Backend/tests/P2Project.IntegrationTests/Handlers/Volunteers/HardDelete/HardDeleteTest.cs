using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using P2Project.Application.Interfaces.Commands;
using P2Project.Application.Volunteers.Commands.HardDelete;
using P2Project.IntegrationTests.Factories;

namespace P2Project.IntegrationTests.Handlers.Volunteers.HardDelete;

public class HardDeleteTest : IntegrationTestBase
{
    private readonly ICommandHandler<Guid, HardDeleteCommand> _sut;
    public HardDeleteTest(IntegrationTestsFactory factory) : base(factory)
    {
        var scope = factory.Services.CreateScope();
        _sut = scope.ServiceProvider
            .GetRequiredService<ICommandHandler<Guid, HardDeleteCommand>>();
    }
    
    [Fact]
    public async Task HardDelete()
    {
        // Arrange
        var volunteerId = await SeedVolunteer();
        var command = new HardDeleteCommand(volunteerId);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().Be(true);
        result.Value.Should().NotBeEmpty();

        var volunteers = _writeDbContext.Volunteers.ToList();
        volunteers.Should().BeEmpty();
        volunteers.Should().HaveCount(0);
    }
}