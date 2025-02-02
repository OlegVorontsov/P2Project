using System.Security.Claims;
using CSharpFunctionalExtensions;
using P2Project.Accounts.Application.Models;
using P2Project.Accounts.Domain;
using P2Project.SharedKernel.Errors;

namespace P2Project.Accounts.Application.Interfaces;

public interface ITokenProvider
{
    public Task<JwtTokenResult> GenerateAccessToken(
        User user, CancellationToken cancellationToken);
    Task<Guid> GenerateRefreshToken(
        User userExist, Guid accessTokenJti, CancellationToken cancellationToken);
    Task<Result<IReadOnlyList<Claim>, Error>> GetUserClaims(
        string jwtToken, CancellationToken cancellationToken);
}