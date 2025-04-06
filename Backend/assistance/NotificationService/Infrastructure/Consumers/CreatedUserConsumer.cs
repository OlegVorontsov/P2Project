using MassTransit;
using NotificationService.Core.EmailMessages.Templates;
using NotificationService.Infrastructure.EmailNotification.EmailManagerImplementations;
using P2Project.Accounts.Agreements.Messages;

namespace NotificationService.Infrastructure.Consumers;

public class CreatedUserConsumer(IConfiguration configuration)
    : IConsumer<CreatedUserEvent>
{
    public async Task Consume(
        ConsumeContext<CreatedUserEvent> context)
    {
        var command = context.Message;
        var emailManager = YandexEmailManager.Build(configuration);
        emailManager.SendMessage(
            command.Email,
            ConfirmationEmailMessage.Subject(),
            ConfirmationEmailMessage.Body(command.UserName, command.EmailConfirmationLink),
            ConfirmationEmailMessage.Styles());
        return;
    }
}