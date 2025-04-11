using CSharpFunctionalExtensions;
using NotificationService.Infrastructure.TelegramNotification;
using P2Project.SharedKernel.Errors;

namespace NotificationService.Application.Telegram.Send;

public class SendTelegramMessageHandler(
    TelegramManager telegramManager)
{
    public async Task<Result<string, ErrorList>> Handle(SendTelegramMessageCommand command, CancellationToken ct)
    {
        var registerResult = await telegramManager.StartRegisterChatId(command.UserId);
        if (registerResult.IsFailure)
            return Errors.General.Failure(registerResult.Error.Message).ToErrorList();
        
        var sentResult = await telegramManager.SendMessage(command.UserId, command.Message);
        if (sentResult.IsFailure)
            return Errors.General.Failure(sentResult.Error.Message).ToErrorList();
        
        return "TelegramMessage sent successfully";
    }
}