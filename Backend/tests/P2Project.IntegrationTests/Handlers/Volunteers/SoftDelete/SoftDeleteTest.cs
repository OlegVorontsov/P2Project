using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using P2Project.Core.Interfaces.Commands;
using P2Project.IntegrationTests.Factories;
using P2Project.Volunteers.Application.Commands.SoftDelete;

namespace P2Project.IntegrationTests.Handlers.Volunteers.SoftDelete;

public class SoftDeleteTest : VolunteerFactory
{
    private readonly ICommandHandler<Guid, SoftDeleteCommand> _sut;
    public SoftDeleteTest(IntegrationTestsFactory factory) : base(factory)
    {
        var scope = factory.Services.CreateScope();
        _sut = scope.ServiceProvider
            .GetRequiredService<ICommandHandler<Guid, SoftDeleteCommand>>();
    }
    
    [Fact]
    public async Task SoftDelete()
    {
        // Arrange
        var volunteerId = await SeedVolunteer();
        var command = new SoftDeleteCommand(volunteerId);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().Be(true);
        result.Value.Should().NotBeEmpty();

        var volunteers = _volunteersReadDbContext.Volunteers.ToList();
        volunteers.Should().NotBeEmpty();
        volunteers.Should().HaveCount(1);
    }
}