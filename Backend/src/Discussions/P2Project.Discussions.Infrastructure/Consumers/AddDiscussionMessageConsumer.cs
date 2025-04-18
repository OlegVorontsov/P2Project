using MassTransit;
using P2Project.Core.Outbox.Messages.VolunteerRequests;
using P2Project.Discussions.Application.DiscussionsManagement.EventHandlers.CreateMessage;

namespace P2Project.Discussions.Infrastructure.Consumers;

public class AddDiscussionMessageConsumer(
    CreateMessageHandler handler) : IConsumer<AddDiscussionMessageEvent>
{
    public async Task Consume(ConsumeContext<AddDiscussionMessageEvent> context)
    {
        await handler.Handle(new CreateMessageCommand(
            context.Message.RequesterId,
            context.Message.SenderId,
            context.Message.Message), context.CancellationToken);
    }
}