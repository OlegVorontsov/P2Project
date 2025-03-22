using MediatR;
using P2Project.SharedKernel.BaseClasses;

namespace P2Project.Core.Extensions;

public static class MediatorExtensions
{
    public static async Task PublishDomainEvents<TId>(
        this IPublisher publisher,
        DomainEntity<TId> entity,
        CancellationToken cancellationToken = default)
        where TId : IComparable<TId>
    {
        foreach (var domainEvent in entity.DomainEvents)
            await publisher.Publish(domainEvent, cancellationToken);

        entity.ClearDomainEvents();
    }
}