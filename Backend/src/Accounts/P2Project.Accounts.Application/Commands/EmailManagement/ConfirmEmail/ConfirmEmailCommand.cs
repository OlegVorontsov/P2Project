using P2Project.Core.Interfaces.Commands;

namespace P2Project.Accounts.Application.Commands.EmailManagement.ConfirmEmail;

public record ConfirmEmailCommand(Guid UserId, string Token) : ICommand;