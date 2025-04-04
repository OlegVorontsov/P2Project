using NotificationService.Infrastructure.EmailNotification.EmailManagerImplementations;

namespace NotificationService.Application.EmailManagement.Send;

public class SendHandler(IConfiguration configuration)
{
    public async Task Handle(SendCommand command, CancellationToken ct)
    {
        var emailManager = YandexEmailManager.Build(configuration);

        emailManager.SendMessage(
            command.RecipientEmail,
            command.Subject,
            command.Body);
    }
}