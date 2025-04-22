using MediatR;
using P2Project.Core.Events;
using P2Project.Core.Interfaces;
using P2Project.Core.Interfaces.Outbox;
using P2Project.Core.Outbox.Messages.VolunteerRequests;
using P2Project.VolunteerRequests.Application.Interfaces;

namespace P2Project.VolunteerRequests.Application.EventHandlers;

public class ApprovedHandler(
    IOutboxRepository outboxRepository) :
    INotificationHandler<ApprovedEvent>
{
    public async Task Handle(
        ApprovedEvent domainEvent,
        CancellationToken cancellationToken)
    {
        var integrationEvent = new CreateVolunteerAccountEvent(
            domainEvent.UserId, domainEvent.UserName);
        
        await outboxRepository.Add(
            integrationEvent, cancellationToken);
    }
}