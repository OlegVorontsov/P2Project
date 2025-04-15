using P2Project.Core.Interfaces.Commands;

namespace NotificationService.Application.Telegram.Send;

public record SendTelegramMessageCommand( 
    Guid UserId,
    string UserTelegramId,
    string Message) : ICommand;