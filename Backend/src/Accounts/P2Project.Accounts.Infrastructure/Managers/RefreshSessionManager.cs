using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using P2Project.Accounts.Application.Interfaces;
using P2Project.Accounts.Domain;
using P2Project.Accounts.Infrastructure.DbContexts;
using P2Project.Core.Interfaces.Caching;
using P2Project.SharedKernel;
using P2Project.SharedKernel.Errors;

namespace P2Project.Accounts.Infrastructure.Managers;

public class RefreshSessionManager(
    ICacheService _cacheService) : IRefreshSessionManager
{
    public async Task<Result<RefreshSession, Error>> GetByRefreshToken(
        Guid refreshToken, CancellationToken cancellationToken)
    {
        var key = Constants.CacheConstants.REFRESH_SESSIONS_PREFIX + refreshToken;
        var session = await _cacheService.GetAsync<RefreshSession>(key, cancellationToken);

        return session ?? Result.Failure<RefreshSession, Error>(Errors.General.NotFound(refreshToken));
    }

    public async Task<Guid> DeleteAsync(RefreshSession refreshSession, CancellationToken cancellationToken)
    {
        var key = Constants.CacheConstants.REFRESH_SESSIONS_PREFIX + refreshSession.RefreshToken;
        await _cacheService.RemoveAsync(key, cancellationToken);
        return refreshSession.RefreshToken;
    }
}