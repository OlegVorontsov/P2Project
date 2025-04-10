using P2Project.Core.Interfaces.Commands;

namespace P2Project.Accounts.Application.Commands.EmailManagement.GenerateEmailConfirmationToken;

public record GenerateEmailConfirmationTokenCommand(Guid UserId) : ICommand;