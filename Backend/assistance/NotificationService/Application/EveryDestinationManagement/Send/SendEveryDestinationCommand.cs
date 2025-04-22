using P2Project.Core.Interfaces.Commands;

namespace NotificationService.Application.EveryDestinationManagement.Send;

public record SendEveryDestinationCommand(
    Guid UserId,
    string Subject,
    string Body,
    string Styles,
    string TelegramMessage) : ICommand;