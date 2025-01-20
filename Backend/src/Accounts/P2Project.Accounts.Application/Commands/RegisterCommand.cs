using P2Project.Core.Interfaces.Commands;

namespace P2Project.Accounts.Application.Commands;

public record RegisterCommand(
    string Email, string UserName, string Password) : ICommand;