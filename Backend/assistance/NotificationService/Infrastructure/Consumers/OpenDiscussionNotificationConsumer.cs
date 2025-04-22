using MassTransit;
using NotificationService.Application.EveryDestinationManagement.Send;
using NotificationService.Application.UserNotificationSettingsManagement.GetByUserId;
using NotificationService.Core.EmailMessages.Templates;
using P2Project.Core.Outbox.Messages.VolunteerRequests;

namespace NotificationService.Infrastructure.Consumers;

public class OpenDiscussionNotificationConsumer(
    GetByUserIdHandler getByUserIdHandler,
    SendEveryDestinationHandler sendEveryDestinationHandler,
    ILogger<OpenDiscussionNotificationConsumer> logger)
    : IConsumer<OpenDiscussionEvent>
{
    public async Task Consume(
        ConsumeContext<OpenDiscussionEvent> context)
    {
        var command = context.Message;
        
        var userNotificationSettings = await getByUserIdHandler.Handle(
            new GetByUserIdQuery(command.ApplicantUserId),
            CancellationToken.None);
        if (userNotificationSettings == null)
        {
            logger.LogError($"NotificationSettings is null user's: {command.ApplicantUserId}");
            return;
        }
        
        var sentResult = await sendEveryDestinationHandler.Handle(new SendEveryDestinationCommand(
                userNotificationSettings.UserId,
                OpenDiscussionEmailTemplate.Subject(),
                OpenDiscussionEmailTemplate.Body(command.UserName),
                OpenDiscussionEmailTemplate.Styles(),
                $"Здравствуйте, {command.UserName}! По Вашей заявке на волонтерство на сайте P2Project открыта дискуссия."),
            CancellationToken.None);
        logger.LogInformation(sentResult);
    }
}