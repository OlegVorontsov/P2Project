using MediatR;
using P2Project.Core.Events;
using P2Project.Core.Interfaces;
using P2Project.Core.Interfaces.Outbox;
using P2Project.Core.Outbox.Messages.VolunteerRequests;
using P2Project.VolunteerRequests.Application.Interfaces;

namespace P2Project.VolunteerRequests.Application.EventHandlers;

public class ReviewStartedHandler(
    IOutboxRepository outboxRepository) :
    INotificationHandler<ReviewStartedEvent>
{
    public async Task Handle(
        ReviewStartedEvent domainEvent,
        CancellationToken cancellationToken)
    {
        var integrationEvent = new OpenDiscussionEvent(
            domainEvent.RequestId,
            domainEvent.AdminId,
            domainEvent.UserId,
            domainEvent.UserName);
        
        await outboxRepository.Add(
            integrationEvent,
            cancellationToken);
    }
}