using MassTransit;
using P2Project.Accounts.Application.EventHandlers.CreateVolunteerAccount;
using P2Project.Core.Outbox.Messages.VolunteerRequests;

namespace P2Project.Accounts.Infrastructure.Consumers;

public class CreateVolunteerAccountConsumer(
    CreateVolunteerAccountHandler handler) : IConsumer<CreateVolunteerAccountEvent>
{
    public async Task Consume(
        ConsumeContext<CreateVolunteerAccountEvent> context)
    {
        await handler.Handle(
            new CreateVolunteerAccountCommand(context.Message.UserId),
            context.CancellationToken);
    }
}