using CSharpFunctionalExtensions;
using NotificationService.Infrastructure.TelegramNotification;
using P2Project.Core.Interfaces.Commands;
using P2Project.SharedKernel.Errors;

namespace NotificationService.Application.Telegram.Send;

public class SendTelegramMessageHandler(
    TelegramManager telegramManager) :
    ICommandHandler<string, SendTelegramMessageCommand>
{
    public async Task<Result<string, ErrorList>> Handle(SendTelegramMessageCommand command, CancellationToken ct)
    {
        var registerResult = await telegramManager.StartRegisterChatId(command.UserId, command.UserTelegramId);
        if (registerResult.IsFailure)
            return Errors.General.Failure("Fail to register telegramChatId").ToErrorList();
        
        var sentResult = await telegramManager.SendMessage(command.UserId, command.Message);
        if (sentResult.IsFailure)
            return Errors.General.Failure("Fail to send TelegramMessage").ToErrorList();
        
        return "TelegramMessage sent successfully";
    }
}