using P2Project.Core.Interfaces.Commands;

namespace P2Project.Accounts.Application.Commands.Unban;

public record UnbanCommand(Guid UserId) : ICommand;