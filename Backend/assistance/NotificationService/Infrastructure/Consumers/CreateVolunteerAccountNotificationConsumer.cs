using MassTransit;
using NotificationService.Application.EveryDestinationManagement.Send;
using NotificationService.Application.UserNotificationSettingsManagement.GetByUserId;
using NotificationService.Core.EmailMessages.Templates;
using P2Project.Core.Outbox.Messages.VolunteerRequests;

namespace NotificationService.Infrastructure.Consumers;

public class CreateVolunteerAccountNotificationConsumer(
    GetByUserIdHandler getByUserIdHandler,
    SendEveryDestinationHandler sendEveryDestinationHandler,
    ILogger<CreateVolunteerAccountNotificationConsumer> logger)
    : IConsumer<CreateVolunteerAccountEvent>
{
    public async Task Consume(ConsumeContext<CreateVolunteerAccountEvent> context)
    {
        var command = context.Message;
        
        var userNotificationSettings = await getByUserIdHandler.Handle(
            new GetByUserIdQuery(command.UserId),
            CancellationToken.None);
        if (userNotificationSettings == null)
        {
            logger.LogError($"NotificationSettings is null user's: {command.UserId}");
            return;
        }
        
        var sentResult = await sendEveryDestinationHandler.Handle(new SendEveryDestinationCommand(
                userNotificationSettings.UserId,
                userNotificationSettings.Email,
                CreateVolunteerAccountEmailTemplate.Subject(),
                CreateVolunteerAccountEmailTemplate.Body(command.UserName),
                CreateVolunteerAccountEmailTemplate.Styles(),
                $"Здравствуйте, {command.UserName}! Рады сообщить, что Ваша заявка на волонтерство на сайте P2Project одобрена."),
            CancellationToken.None);
        logger.LogInformation(sentResult);
    }
}