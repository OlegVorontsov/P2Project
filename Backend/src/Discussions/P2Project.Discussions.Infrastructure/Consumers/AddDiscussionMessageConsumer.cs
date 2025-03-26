using MassTransit;
using Microsoft.AspNetCore.Mvc;
using P2Project.Discussions.Application.DiscussionsManagement.Commands.AddMessageInDiscussionById;
using P2Project.Discussions.Application.DiscussionsManagement.EventHandlers.CreateMessage;
using P2Project.VolunteerRequests.Agreements.Messages;

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