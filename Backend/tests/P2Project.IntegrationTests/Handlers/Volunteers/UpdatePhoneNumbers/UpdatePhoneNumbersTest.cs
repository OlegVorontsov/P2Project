using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using P2Project.Core.Interfaces.Commands;
using P2Project.IntegrationTests.Extensions;
using P2Project.IntegrationTests.Factories;
using P2Project.Volunteers.Application.Commands.UpdatePhoneNumbers;

namespace P2Project.IntegrationTests.Handlers.Volunteers.UpdatePhoneNumbers;

public class UpdatePhoneNumbersTest : VolunteerFactory
{
    private readonly ICommandHandler<Guid, UpdatePhoneNumbersCommand> _sut;

    public UpdatePhoneNumbersTest(IntegrationTestsFactory factory) : base(factory)
    {
        var scope = factory.Services.CreateScope();
        _sut = scope.ServiceProvider
            .GetRequiredService<ICommandHandler<Guid, UpdatePhoneNumbersCommand>>();
    }
    
    [Fact]
    public async Task UpdatePhoneNumbers()
    {
        // Arrange
        var volunteerId = await SeedVolunteer();
        var command = _fixture.FakeUpdatePhoneNumbersCommand(volunteerId);

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