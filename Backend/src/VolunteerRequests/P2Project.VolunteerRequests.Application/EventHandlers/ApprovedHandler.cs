using MassTransit;
using MassTransit.DependencyInjection;
using MediatR;
using P2Project.Accounts.Application.Interfaces;
using P2Project.Core.Events;
using P2Project.VolunteerRequests.Agreements.Messages;

namespace P2Project.VolunteerRequests.Application.EventHandlers;

public class ApprovedHandler(
    Bind<IAccountsMessageBus, IPublishEndpoint> publishEndpoint) :
    INotificationHandler<ApprovedEvent>
{
    public async Task Handle(
        ApprovedEvent domainEvent,
        CancellationToken cancellationToken)
    {
        await publishEndpoint.Value.Publish(
            new CreateVolunteerAccountEvent(
                domainEvent.UserId), cancellationToken);
    }
}