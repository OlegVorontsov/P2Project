using CSharpFunctionalExtensions;
using NotificationService.Application.EveryDestinationManagement.Send;
using NotificationService.Application.Interfaces;
using NotificationService.Domain;
using NotificationService.Infrastructure.TelegramNotification;
using P2Project.SharedKernel.Errors;

namespace NotificationService.Application.SendersManagement.Senders;

public class TelegramSender(TelegramManager telegramManager) : INotificationSender
{
    public async Task<Result<string, ErrorList>> SendAsync(
        UserNotificationSettings userNotificationSetting,
        SendEveryDestinationCommand command,
        CancellationToken ct)
    {
        var registerResult = await telegramManager.StartRegisterChatId(
            userNotificationSetting.UserId,
            userNotificationSetting.TelegramSettings!.UserId);
        if (registerResult.IsFailure)
            return Errors.General.Failure("Fail to register telegramChatId").ToErrorList();
        
        var sentResult = await telegramManager.SendMessage(command.UserId, command.TelegramMessage);
        if (sentResult.IsFailure)
            return Errors.General.Failure("Fail to send TelegramMessage").ToErrorList();
        
        return $"TelegramMessage sent successfully to: {userNotificationSetting.TelegramSettings!.UserId}\n";
    }
    
    public bool CanSend(
        UserNotificationSettings userNotificationSetting,
        CancellationToken cancellationToken) =>
        userNotificationSetting.TelegramSettings != null &&
        !string.IsNullOrWhiteSpace(userNotificationSetting.TelegramSettings.UserId);
}