using MassTransit;
using MassTransit.DependencyInjection;
using MediatR;
using P2Project.Core.Events;
using P2Project.Discussions.Application.Interfaces;
using P2Project.VolunteerRequests.Agreements.Messages;

namespace P2Project.VolunteerRequests.Application.EventHandlers;

public class CreateMessageHandler(
    Bind<IDiscussionMessageBus, IPublishEndpoint> publishEndpoint) :
    INotificationHandler<CreateMessageEvent>
{
    public async Task Handle(
        CreateMessageEvent domainEvent,
        CancellationToken cancellationToken)
    {
        await publishEndpoint.Value.Publish(
            new AddDiscussionMessageEvent(
                domainEvent.RequestId,
                domainEvent.SenderId,
                domainEvent.Message), cancellationToken);
    }
}