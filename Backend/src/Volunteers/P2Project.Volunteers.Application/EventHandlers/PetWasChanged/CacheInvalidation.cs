using MediatR;
using P2Project.Core.Interfaces.Caching;
using P2Project.SharedKernel;
using P2Project.Volunteers.Domain.Events;

namespace P2Project.Volunteers.Application.EventHandlers.PetWasChanged;

public class CacheInvalidation(ICacheService cacheService) :
    INotificationHandler<PetWasChangedEvent>
{
    public async Task Handle(
        PetWasChangedEvent domainEvent,
        CancellationToken cancellationToken)
    {
        await cacheService.RemoveByPrefixAsync(Constants.CacheConstants.PETS_PREFIX, cancellationToken);
    }
}