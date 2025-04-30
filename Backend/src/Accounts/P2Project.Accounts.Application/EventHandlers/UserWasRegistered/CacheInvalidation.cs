using MediatR;
using P2Project.Accounts.Domain.Events;
using P2Project.Core.Interfaces.Caching;
using P2Project.SharedKernel;

namespace P2Project.Accounts.Application.EventHandlers.UserWasRegistered;

public class CacheInvalidation(ICacheService cacheService) :
    INotificationHandler<UserWasRegisteredEvent>
{
    public async Task Handle(
        UserWasRegisteredEvent domainEvent,
        CancellationToken cancellationToken)
    {
        await cacheService.RemoveByPrefixAsync(Constants.CacheConstants.USERS_PREFIX, cancellationToken);
    }
}