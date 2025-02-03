using P2Project.Core.Interfaces.Commands;

namespace P2Project.Accounts.Application.Commands.RefreshTokens;

public record RefreshTokensCommand(string AccessToken, Guid RefreshToken) : ICommand;