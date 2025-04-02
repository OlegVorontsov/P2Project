using NotificationService.Core.Options;

namespace NotificationService.Infrastructure.EmailNotification.EmailManagerImplementations;

public class GoogleEmailManager
{
    private static readonly string host = "smtp.gmail.com";
    private static readonly int port = 587;

    public static EmailManager Build(IConfiguration configuration)
    {
        var googleOption = configuration.GetSection(EmailOptions.GOOGLE).Get<EmailOptions>();
        var senderEmail = googleOption.Email;
        var senderPassword = googleOption.Password;

        return EmailManager.Build(senderEmail, senderPassword, host, port);
    }
}