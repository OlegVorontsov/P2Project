using MassTransit;
using NotificationService.Application.EventHandlers;
using NotificationService.Application.UserNotificationSettingsManagement.GetByUserId;
using NotificationService.Core.EmailMessages.Templates;
using NotificationService.Infrastructure.EmailNotification.EmailManagerImplementations;
using P2Project.Accounts.Agreements.Messages;

namespace NotificationService.Infrastructure.Consumers;

public class CreatedUserConsumer(
    IConfiguration configuration,
    GetByUserIdHandler getByUserIdHandler,
    ConfirmationEmailHandler confirmationEmailHandler,
    ILogger<CreatedUserConsumer> logger)
    : IConsumer<CreatedUserEvent>
{
    public async Task Consume(
        ConsumeContext<CreatedUserEvent> context)
    {
        var command = context.Message;
        var emailManager = YandexEmailManager.Build(configuration);
        
        var userNotificationSettings = await getByUserIdHandler.Handle(
            command.UserId, CancellationToken.None);

        if (userNotificationSettings != null &&
            userNotificationSettings.IsEmailSend.HasValue &&
            userNotificationSettings.IsEmailSend.Value)
        {
            var sentResult = emailManager.SendMessage(
                command.Email,
                RegisterUserEmailMessage.Subject(),
                RegisterUserEmailMessage.Body(command.UserName),
                RegisterUserEmailMessage.Styles());
            if (sentResult.IsFailure)
                logger.LogError(sentResult.Error.Message);
            else
                logger.LogInformation($"RegisterUserEmailMessage sent successfully to: {command.Email}");
        }
        await confirmationEmailHandler.Handle(command.UserId, CancellationToken.None);
    }
}