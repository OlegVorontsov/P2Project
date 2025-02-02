using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using P2Project.Accounts.Application.Interfaces;
using P2Project.Accounts.Domain;
using P2Project.Accounts.Infrastructure.DbContexts;
using P2Project.SharedKernel.Errors;

namespace P2Project.Accounts.Infrastructure.Managers;

public class RefreshSessionManager(
    AuthorizationDbContext dbContext) : IRefreshSessionManager
{
    public async Task<Result<RefreshSession, Error>> GetByRefreshToken(
        Guid refreshToken, CancellationToken cancellationToken)
    {
        var refreshSession = await dbContext.RefreshSessions
            .Include(r => r.User)
            .FirstOrDefaultAsync(t => t.RefreshToken == refreshToken, cancellationToken);
        
        if(refreshSession is null)
            return Errors.General.NotFound(refreshToken);
        
        return refreshSession;
    }
    
    public void Delete(RefreshSession refreshSession)
    {
        dbContext.RefreshSessions.Remove(refreshSession);
    }
}