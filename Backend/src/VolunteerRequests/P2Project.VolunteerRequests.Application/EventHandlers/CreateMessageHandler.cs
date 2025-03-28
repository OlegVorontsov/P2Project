using MediatR;
using P2Project.Core.Events;
using P2Project.VolunteerRequests.Agreements.Messages;
using P2Project.VolunteerRequests.Application.Interfaces;

namespace P2Project.VolunteerRequests.Application.EventHandlers;

public class CreateMessageHandler(
    IOutboxRepository outboxRepository) :
    INotificationHandler<CreateMessageEvent>
{
    public async Task Handle(
        CreateMessageEvent domainEvent,
        CancellationToken cancellationToken)
    {
        var integrationEvent = new AddDiscussionMessageEvent(
            domainEvent.RequestId,
            domainEvent.SenderId,
            domainEvent.Message);
        
        await outboxRepository.Add(
            integrationEvent,
            cancellationToken);
    }
}