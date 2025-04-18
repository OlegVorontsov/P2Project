using MassTransit;
using NotificationService.Application.EveryDestinationManagement.Send;
using NotificationService.Application.UserNotificationSettingsManagement.GetByUserId;
using NotificationService.Core.EmailMessages.Templates;
using P2Project.Core.Outbox.Messages.VolunteerRequests;

namespace NotificationService.Infrastructure.Consumers;

public class AddDiscussionMessageNotificationConsumer(
    GetByUserIdHandler getByUserIdHandler,
    SendEveryDestinationHandler sendEveryDestinationHandler,
    ILogger<CreatedUserConsumer> logger)
    : IConsumer<AddDiscussionMessageEvent>
{
    public async Task Consume(
        ConsumeContext<AddDiscussionMessageEvent> context)
    {
        var command = context.Message;
        
        var userNotificationSettings = await getByUserIdHandler.Handle(
            new GetByUserIdQuery(command.RequestUserId),
            CancellationToken.None);
        if (userNotificationSettings == null)
        {
            logger.LogError($"NotificationSettings is null user's: {command.RequestUserId}");
            return;
        }
        
        var sentResult = await sendEveryDestinationHandler.Handle(new SendEveryDestinationCommand(
                userNotificationSettings.UserId,
                userNotificationSettings.Email,
                AddDiscussionMessageEmailTemplate.Subject(),
                AddDiscussionMessageEmailTemplate.Body(command.UserName, command.Message),
                AddDiscussionMessageEmailTemplate.Styles(),
                $"Здравствуйте, {command.UserName}! В дискуссии по Вашей заявке на волонтерство на сайте P2Project произошли изменения. Опубликовано сообщение: {command.Message}"),
            CancellationToken.None);
        logger.LogInformation(sentResult);
    }
}