using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using P2Project.Accounts.Agreements.Responses;
using P2Project.Accounts.Application.Interfaces;
using P2Project.Accounts.Domain;
using P2Project.Core;
using P2Project.Core.Interfaces;
using P2Project.Core.Interfaces.Commands;
using P2Project.Framework.Authorization;
using P2Project.SharedKernel.Errors;

namespace P2Project.Accounts.Application.Commands.RefreshTokens;

public class RefreshTokensHandler :
    ICommandHandler<LoginResponse, RefreshTokensCommand>
{
    private readonly IRefreshSessionManager _refreshSessionManager;
    private readonly ITokenProvider _tokenProvider;
    private readonly UserManager<User> _userManager;

    public RefreshTokensHandler(
        IRefreshSessionManager refreshSessionManager,
        ITokenProvider tokenProvider,
        UserManager<User> userManager)
    {
        _refreshSessionManager = refreshSessionManager;
        _tokenProvider = tokenProvider;
        _userManager = userManager;
    }

    public async Task<Result<LoginResponse, ErrorList>> Handle(
        RefreshTokensCommand command,
        CancellationToken cancellationToken)
    {
        var oldRefreshSession = await _refreshSessionManager
            .GetByRefreshToken(command.RefreshToken, cancellationToken);
        if(oldRefreshSession.IsFailure)
            return Errors.General.NotFound(command.RefreshToken).ToErrorList();

        var userClaims = await _tokenProvider.GetUserClaims(command.AccessToken, cancellationToken);
        if (userClaims.IsFailure)
            return Errors.AccountError.InvalidToken().ToErrorList();

        var userIdString = userClaims.Value.FirstOrDefault(c => c.Type == CustomClaims.Id)?.Value;
        if(!Guid.TryParse(userIdString, out var userId))
            return Errors.General.Failure(userIdString).ToErrorList();
        if(oldRefreshSession.Value.UserId != userId)
            return Errors.AccountError.InvalidToken().ToErrorList();

        var userJtiString = userClaims.Value.FirstOrDefault(c => c.Type == CustomClaims.Jti)?.Value;
        if(!Guid.TryParse(userJtiString, out var userJtiGuid))
            return Errors.General.Failure(userJtiString).ToErrorList();
        if(oldRefreshSession.Value.Jti != userJtiGuid)
            return Errors.AccountError.InvalidToken().ToErrorList();

        await _refreshSessionManager.DeleteAsync(oldRefreshSession.Value, cancellationToken);

        var user = await _userManager.FindByIdAsync(userIdString);

        var accessToken = await _tokenProvider
            .GenerateAccessToken(user!, cancellationToken);
        var refreshToken = await _tokenProvider
            .GenerateRefreshToken(user!, accessToken.Jti, cancellationToken);

        return new LoginResponse(accessToken.AccessToken, refreshToken);
    }
}