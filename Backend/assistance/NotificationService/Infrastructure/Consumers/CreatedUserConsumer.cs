using MassTransit;
using NotificationService.Application.EventHandlers;
using NotificationService.Application.EveryDestinationManagement.Send;
using NotificationService.Application.UserNotificationSettingsManagement.SetByUserId;
using NotificationService.Core.Dtos;
using NotificationService.Core.EmailMessages.Templates;
using P2Project.Accounts.Agreements.Messages;

namespace NotificationService.Infrastructure.Consumers;

public class CreatedUserConsumer(
    SetByUserIdHandler setByUserIdHandler,
    SendEveryDestinationHandler sendEveryDestinationHandler,
    ConfirmationEmailHandler confirmationEmailHandler,
    ILogger<CreatedUserConsumer> logger)
    : IConsumer<CreatedUserEvent>
{
    public async Task Consume(
        ConsumeContext<CreatedUserEvent> context)
    {
        var command = context.Message;
        
        var newUserNotificationSettings = new SentNotificationSettings(
            true, command.TelegramUserId, true);
        await setByUserIdHandler.Handle(
            new SetByUserIdCommand(command.UserId, newUserNotificationSettings),
            CancellationToken.None);
        
        var sentResult = await sendEveryDestinationHandler.Handle(new SendEveryDestinationCommand(
            command.UserId,
            command.Email,
            RegisterUserEmailMessage.Subject(),
            RegisterUserEmailMessage.Body(command.UserName),
            RegisterUserEmailMessage.Styles(),
            $"Здравствуйте, {command.UserName}! Рады приветствовать Вас на сайте P2Project. В ближайшее время Вам на почту {command.Email} придет письмо для подтверждения email."
            ), CancellationToken.None);
        logger.LogInformation(sentResult);
        
        await confirmationEmailHandler.Handle(
            new ConfirmationEmailCommand(command.UserId),
            CancellationToken.None);
    }
}