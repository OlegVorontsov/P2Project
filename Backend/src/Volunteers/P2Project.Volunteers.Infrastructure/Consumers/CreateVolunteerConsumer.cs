using MassTransit;
using Microsoft.Extensions.Logging;
using P2Project.Accounts.Agreements;
using P2Project.Core.Dtos.Volunteers;
using P2Project.Core.Outbox.Messages.VolunteerRequests;
using P2Project.Volunteers.Application.Commands.Create;

namespace P2Project.Volunteers.Infrastructure.Consumers;

public class CreateVolunteerConsumer(
    IAccountsAgreements accountsAgreements,
    CreateHandler createVolunteerHandler,
    ILogger<CreateVolunteerConsumer> logger) : IConsumer<CreateVolunteerAccountEvent>
{
    public async Task Consume(ConsumeContext<CreateVolunteerAccountEvent> context)
    {
        var userDto = await accountsAgreements.GetUserInfo(
            context.Message.UserId,
            CancellationToken.None);
        if (userDto.IsFailure)
        {
            logger.LogError($"User with id: {context.Message.UserId} not found");
            return;
        }

        var volunteerInfoDto = new VolunteerInfoDto(
            context.Message.Age,
            context.Message.Grade);

        await createVolunteerHandler.Handle(
            new CreateCommand(volunteerInfoDto, context.Message.Gender, null, null),
            context.CancellationToken);
    }
}