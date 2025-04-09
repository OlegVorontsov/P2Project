using MassTransit;
using NotificationService.Application.EventHandlers;
using NotificationService.Core.EmailMessages.Templates;
using NotificationService.Infrastructure.EmailNotification.EmailManagerImplementations;
using P2Project.Accounts.Agreements.Messages;

namespace NotificationService.Infrastructure.Consumers;

public class CreatedUserConsumer(
    IConfiguration configuration,
    ConfirmationEmailHandler handler,
    ILogger<CreatedUserConsumer> logger)
    : IConsumer<CreatedUserEvent>
{
    public async Task Consume(
        ConsumeContext<CreatedUserEvent> context)
    {
        var command = context.Message;
        var emailManager = YandexEmailManager.Build(configuration);
        var sentResult = emailManager.SendMessage(
            command.Email,
            RegisterUserEmailMessage.Subject(),
            RegisterUserEmailMessage.Body(command.UserName),
            RegisterUserEmailMessage.Styles());
        
        if (sentResult.IsFailure)
            logger.LogError(sentResult.Error.Message);
        else
        {
            logger.LogInformation($"RegisterUserEmailMessage sent successfully to: {command.Email}");
            await handler.Handle(command.UserId, CancellationToken.None);
        }
    }
}