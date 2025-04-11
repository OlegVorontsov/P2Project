using NotificationService.Application.EmailManagement.Send;
using NotificationService.Application.Telegram.Send;
using NotificationService.Domain;
using NotificationService.Infrastructure;
using NotificationService.Infrastructure.Repositories;

namespace NotificationService.Application.EveryDestinationManagement.Send;

public class SendEveryDestinationHandler(
    NotificationRepository repository,
    UnitOfWork unitOfWork,
    SendEmailHandler sendEmailHandler,
    SendTelegramMessageHandler sendTelegramMessageHandler,
    ILogger<SendEveryDestinationHandler> logger)
{
    public async Task<string> Handle(SendEveryDestinationCommand command, CancellationToken ct)
    {
        var notificationSettingsExist = await repository.Get(command.UserId, ct);
        
        if (notificationSettingsExist is null)
        {
            notificationSettingsExist = UserNotificationSettings.New(command.UserId);
            var transaction = await unitOfWork.BeginTransaction(CancellationToken.None);
            await unitOfWork.SaveChanges(CancellationToken.None);
            transaction.Commit();
        }

        var sentEveryDestinationResult = string.Empty;
        
        if (notificationSettingsExist.IsEmailSend.HasValue &&
            notificationSettingsExist.IsEmailSend.Value)
        {
            var sentEmailResult = await sendEmailHandler.Handle(
                new SendEmailCommand(
                    command.Email,
                    command.Subject,
                    command.Body,
                    command.Styles), ct);
            if (sentEmailResult.IsFailure)
            {
                sentEveryDestinationResult += "Email send failed; ";
                logger.LogError($"Failed to send email to: {command.Email}");
            }
            else
                logger.LogInformation($"Email sent successfully to: {command.Email}");
        }
        
        if (notificationSettingsExist.IsTelegramSend.HasValue &&
            notificationSettingsExist.IsTelegramSend.Value)
        {
            var sentTelegramMessageResult = await sendTelegramMessageHandler.Handle(
                new SendTelegramMessageCommand(
                    command.UserId,
                    command.Body), ct);
            if (sentTelegramMessageResult.IsFailure)
            {
                sentEveryDestinationResult += "TelegramMessage send failed; ";
                logger.LogError($"Failed to send TelegramMessage for user: {command.UserId}");
            }
            else
                logger.LogInformation($"TelegramMessage sent successfully for user: {command.UserId}");
        }
        
        if (notificationSettingsExist.IsWebSend.HasValue &&
            notificationSettingsExist.IsWebSend.Value)
        {
            logger.LogInformation($"Web sent successfully for user: {command.UserId}");
        }
        
        return sentEveryDestinationResult;
    }
}