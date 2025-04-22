using NotificationService.Application.SendersManagement;
using NotificationService.Infrastructure.Repositories;
using P2Project.Core.Interfaces.Commands;

namespace NotificationService.Application.EveryDestinationManagement.Send;

public class SendEveryDestinationHandler(
    NotificationRepository repository,
    SendersFactory sendersFactory,
    ILogger<SendEveryDestinationHandler> logger) :
    ICommandResponseHandler<string, SendEveryDestinationCommand>
{
    public async Task<string> Handle(SendEveryDestinationCommand command, CancellationToken ct)
    {
        var notificationSettingsExist = await repository.Get(command.UserId, ct);
        
        var sentEveryDestinationResult = string.Empty;
        
        if (notificationSettingsExist is null)
        {
            sentEveryDestinationResult += "NotificationSettings is null";
            logger.LogError($"NotificationSettings is null user's: {command.UserId}");
            return sentEveryDestinationResult;
        }
        
        var senders = sendersFactory.GetSenders(notificationSettingsExist, ct);

        foreach (var sender in senders)
        {
            var sentResult = await sender.SendAsync(command, ct);
            if (sentResult.IsSuccess)
                sentEveryDestinationResult += sentResult.Value;
        }
        
        /*if (notificationSettingsExist.Email != null)
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
        
        if (notificationSettingsExist.TelegramSettings != null)
        {
            var sentTelegramMessageResult = await sendTelegramMessageHandler.Handle(
                new SendTelegramMessageCommand(
                    command.UserId,
                    notificationSettingsExist.TelegramSettings.UserId,
                    command.TelegramMessage), ct);
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
        }*/
        
        return sentEveryDestinationResult;
    }
}