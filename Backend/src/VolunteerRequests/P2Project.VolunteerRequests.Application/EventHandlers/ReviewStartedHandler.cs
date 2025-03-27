using MassTransit;
using MassTransit.DependencyInjection;
using MediatR;
using P2Project.Core.Events;
using P2Project.Discussions.Application.Interfaces;
using P2Project.VolunteerRequests.Agreements.Messages;

namespace P2Project.VolunteerRequests.Application.EventHandlers;

public class ReviewStartedHandler(
    Bind<IDiscussionMessageBus, IPublishEndpoint> publishEndpoint) :
    INotificationHandler<ReviewStartedEvent>
{
    public async Task Handle(
        ReviewStartedEvent domainEvent,
        CancellationToken cancellationToken)
    {
        await publishEndpoint.Value.Publish(
            new OpenDiscussionEvent(
                domainEvent.RequestId,
                domainEvent.AdminId,
                domainEvent.UserId), cancellationToken);
    }
}