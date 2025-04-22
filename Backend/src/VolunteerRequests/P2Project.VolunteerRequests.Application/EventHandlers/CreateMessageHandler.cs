using MediatR;
using P2Project.Core.Events;
using P2Project.Core.Interfaces.Outbox;
using P2Project.Core.Outbox.Messages.VolunteerRequests;

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
            domainEvent.RequestUserId,
            domainEvent.UserName,
            domainEvent.Message);
        
        await outboxRepository.Add(
            integrationEvent,
            cancellationToken);
    }
}