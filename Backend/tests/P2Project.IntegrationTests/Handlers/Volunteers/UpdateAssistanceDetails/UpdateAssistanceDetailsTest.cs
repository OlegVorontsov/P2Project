using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using P2Project.Core.Interfaces.Commands;
using P2Project.IntegrationTests.Extensions;
using P2Project.IntegrationTests.Factories;
using P2Project.Volunteers.Application.Commands.UpdateAssistanceDetails;

namespace P2Project.IntegrationTests.Handlers.Volunteers.UpdateAssistanceDetails;

public class UpdateAssistanceDetailsTest : VolunteerFactory
{
    private readonly ICommandHandler<Guid, UpdateAssistanceDetailsCommand> _sut;
    public UpdateAssistanceDetailsTest(IntegrationTestsFactory factory) : base(factory)
    {
        var scope = factory.Services.CreateScope();
        _sut = scope.ServiceProvider
            .GetRequiredService<ICommandHandler<Guid, UpdateAssistanceDetailsCommand>>();
    }
    
    [Fact]
    public async Task UpdateAssistanceDetails()
    {
        // Arrange
        var volunteerId = await SeedVolunteer();
        var command = _fixture.FakeUpdateAssistanceDetailsCommand(volunteerId);

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