using P2Project.Core.Interfaces.Commands;

namespace P2Project.Accounts.Application.Commands.Login;

public record LoginCommand(string Email, string Password) : ICommand;