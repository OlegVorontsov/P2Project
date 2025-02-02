using CSharpFunctionalExtensions;
using P2Project.Accounts.Domain;
using P2Project.SharedKernel.Errors;

namespace P2Project.Accounts.Application.Interfaces;

public interface IRefreshSessionManager
{
    public Task<Result<RefreshSession, Error>> GetByRefreshToken(
        Guid refreshToken, CancellationToken cancellationToken);
    public void Delete(RefreshSession refreshSession);
}