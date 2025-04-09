using MassTransit;
using NotificationService.Core.EmailMessages.Templates;
using NotificationService.Infrastructure.EmailNotification.EmailManagerImplementations;
using P2Project.Accounts.Agreements.Messages;

namespace NotificationService.Infrastructure.Consumers;

public class ConfirmUserEmailConsumer(
    IConfiguration configuration,
    ILogger<ConfirmUserEmailConsumer> logger)
    : IConsumer<ConfirmedUserEmailEvent>
{
    public async Task Consume(ConsumeContext<ConfirmedUserEmailEvent> context)
    {
        var command = context.Message;
        var emailManager = YandexEmailManager.Build(configuration);
        var sentResult = emailManager.SendMessage(
            command.Email,
            ConfirmationEmailMessage.Subject(),
            ConfirmationEmailMessage.Body(command.UserName, command.EmailConfirmationLink),
            ConfirmationEmailMessage.Styles());
        
        if (sentResult.IsFailure)
            logger.LogError(sentResult.Error.Message);
        else
            logger.LogInformation($"ConfirmationEmailMessage sent successfully to: {command.Email}");
    }
}