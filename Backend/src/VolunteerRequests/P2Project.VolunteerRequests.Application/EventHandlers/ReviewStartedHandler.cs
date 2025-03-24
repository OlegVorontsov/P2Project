using MassTransit;
using MassTransit.DependencyInjection;
using MediatR;
using P2Project.Core.Events;
using P2Project.Discussions.Application.Interfaces;
using P2Project.VolunteerRequests.Agreements.Messages;

namespace P2Project.VolunteerRequests.Application.EventHandlers;

public class ReviewStartedHandler :
    INotificationHandler<ReviewStartedEvent>
{
    private readonly Bind<IDiscussionMessageBus, IPublishEndpoint> _publishEndpoint;

    public ReviewStartedHandler(Bind<IDiscussionMessageBus, IPublishEndpoint> publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    public async Task Handle(
        ReviewStartedEvent domainEvent,
        CancellationToken cancellationToken)
    {
        await _publishEndpoint.Value.Publish(
            new VolunteerRequestReviewStartedEvent(
                domainEvent.RequestId,
                domainEvent.AdminId,
                domainEvent.UserId), cancellationToken);
    }
}