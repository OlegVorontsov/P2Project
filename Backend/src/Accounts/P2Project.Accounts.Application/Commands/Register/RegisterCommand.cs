using P2Project.Core.Dtos.Volunteers;
using P2Project.Core.Interfaces.Commands;

namespace P2Project.Accounts.Application.Commands.Register;

public record RegisterCommand(
    FullNameDto FullName,
    string Email,
    string UserName,
    string Password,
    string TelegramUserId) : ICommand;