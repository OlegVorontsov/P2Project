using MediatR;
using P2Project.Core.Events;
using P2Project.Core.Interfaces.Outbox;
using P2Project.Core.Outbox.Messages.VolunteerRequests;

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
            domainEvent.UserId,
            domainEvent.UserName,
            domainEvent.Age,
            domainEvent.Grade,
            domainEvent.Gender);

        await outboxRepository.Add(
            integrationEvent, cancellationToken);
    }
}