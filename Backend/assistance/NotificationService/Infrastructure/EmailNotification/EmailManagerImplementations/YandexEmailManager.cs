using NotificationService.Core.Options;

namespace NotificationService.Infrastructure.EmailNotification.EmailManagerImplementations;

public class YandexEmailManager
{
    private static readonly string host = "smtp.yandex.ru";
    private static readonly int port = 465; 
    public static EmailManager Build(IConfiguration configuration)
    {

        var yandexOption = configuration.GetSection(EmailOptions.GOOGLE).Get<EmailOptions>();
        var senderEmail = yandexOption.Email;
        var senderPassword = yandexOption.Password;

        return EmailManager.Build(senderEmail, senderPassword, host, port);
    }
}