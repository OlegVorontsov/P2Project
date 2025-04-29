using MediatR;
using P2Project.Accounts.Domain.Events;
using P2Project.Core.Interfaces.Caching;
using P2Project.SharedKernel;

namespace P2Project.Accounts.Application.EventHandlers.UserWasChanged;

public class CacheInvalidation(ICacheService cacheService) :
    INotificationHandler<UserWasChangedEvent>
{
    public async Task Handle(
        UserWasChangedEvent domainEvent,
        CancellationToken cancellationToken)
    {
        await cacheService.RemoveByPrefixAsync(Constants.CacheConstants.USERS_PREFIX, cancellationToken);
    }
}