using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using P2Project.Core.Interfaces.Commands;
using P2Project.IntegrationTests.Factories;
using P2Project.Volunteers.Application.Commands.HardDelete;

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

        var volunteers = _volunteersWriteDbContext.Volunteers.ToList();
        volunteers.Should().BeEmpty();
        volunteers.Should().HaveCount(0);
    }
}