using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using P2Project.Core.Interfaces.Commands;
using P2Project.IntegrationTests.Extensions;
using P2Project.IntegrationTests.Factories;
using P2Project.Volunteers.Application.Commands.Create;

namespace P2Project.IntegrationTests.Handlers.Volunteers.CreateVolunteer;

public class CreateVolunteerTest : VolunteerFactory
{
    private readonly ICommandHandler<Guid, CreateCommand> _sut;
    
    public CreateVolunteerTest(IntegrationTestsFactory factory) : base(factory)
    {
        var scope = factory.Services.CreateScope();
        _sut = scope.ServiceProvider
            .GetRequiredService<ICommandHandler<Guid, CreateCommand>>();
    }
    
    [Fact]
    public async Task CreateVolunteerInDatabase()
    {
        // Arrange
        var command = _fixture.FakeCreateVolunteerCommand();

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