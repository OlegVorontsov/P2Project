using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using P2Project.Application.Interfaces.Commands;
using P2Project.Application.Volunteers.Commands.UpdateMainInfo;
using P2Project.IntegrationTests.Extensions;
using P2Project.IntegrationTests.Factories;

namespace P2Project.IntegrationTests.Handlers.Volunteers.UpdateMainInfo;

public class UpdateMainInfoTest : IntegrationTestBase
{
    private readonly ICommandHandler<Guid, UpdateMainInfoCommand> _sut;

    public UpdateMainInfoTest(IntegrationTestsFactory factory) : base(factory)
    {
        var scope = factory.Services.CreateScope();
        _sut = scope.ServiceProvider
            .GetRequiredService<ICommandHandler<Guid, UpdateMainInfoCommand>>();
    }

    [Fact]
    public async Task UpdateMainInfo()
    {
        // Arrange
        var volunteerId = await SeedVolunteer();
        var command = _fixture.FakeUpdateMainInfoCommand(volunteerId);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().Be(true);
        result.Value.Should().NotBeEmpty();

        var volunteers = _writeDbContext.Volunteers.ToList();
        volunteers.Should().NotBeEmpty();
        volunteers.Should().HaveCount(1);
    }
}