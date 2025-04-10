using NotificationService.Core.Options;

namespace NotificationService.Infrastructure.EmailNotification.EmailManagerImplementations;

public class YandexEmailManager : IEmailManager
{
    private static readonly string host = "smtp.yandex.ru";
    private static readonly int port = 465;
    public static EmailManager Build(IConfiguration configuration)
    {

        var yandexOption = configuration.GetSection(EmailOptions.YANDEX).Get<EmailOptions>();
        var senderEmail = yandexOption.Email;
        var senderPassword = yandexOption.Password;

        return EmailManager.Build(senderEmail, senderPassword, host, port);
    }
}