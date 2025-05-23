using CSharpFunctionalExtensions;
using MailKit.Net.Smtp;
using MimeKit;
using MimeKit.Text;
using NotificationService.Core;
using NotificationService.Core.EmailMessages;
using P2Project.SharedKernel.Errors;

namespace NotificationService.Infrastructure.EmailNotification;

public class EmailManager
{
    private readonly string _host;
    private readonly int _port;
    private readonly string _senderEmail = "email@email";
    private readonly string _senderPassword = "password";

    private EmailManager(string senderEmail, string senderPassword, string host, int port)
    {
        _host = host;
        _port = port;
        _senderEmail = senderEmail;
        _senderPassword = senderPassword;
    }

    public UnitResult<Error> SendMessage(
        string recipientEmail,
        string subject,
        string body,
        string styles = "",
        string senderEmailName = Constants.SENDER_EMAIL_NAME)
    {
        var emailMessage = EmailMessageConstructor.Build(styles, body);
        
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(senderEmailName, _senderEmail));
        message.To.Add(new MailboxAddress(string.Empty, recipientEmail));
        message.Subject = subject;
        message.Body = new TextPart(TextFormat.Html)
        {
            Text = emailMessage
        };
        try
        {
            using var client = new SmtpClient();
            client.Connect(_host, _port);
            client.Authenticate(_senderEmail, _senderPassword);
            client.Send(message);
            client.Disconnect(true);
            return Result.Success<Error>();
        }
        catch (Exception ex)
        {
            return Errors.General.Failure(ex.Message);
        }
    }

    public static EmailManager Build(
        string senderEmail,
        string senderPassword,
        string host,
        int port)
    {
        var manager = new EmailManager(senderEmail, senderPassword, host, port);
        return manager;
    }
}