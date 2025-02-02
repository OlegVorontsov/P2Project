using P2Project.Accounts.Application.Commands.RefreshTokens;

namespace P2Project.Accounts.Web.Requests;

public record RefreshTokensRequest(string AccessToken, Guid RefreshToken)
{
    public RefreshTokensCommand ToCommand() =>
        new(AccessToken, RefreshToken);
}
