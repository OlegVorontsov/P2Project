using System.Net;
using System.Net.Mail;

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

    public void SendMessage(string recipientEmail, string subject, string body)
    {
        var smtpClient = new SmtpClient(_host)
        {
            Port = _port,
            Credentials = new NetworkCredential(_senderEmail, _senderPassword),
            EnableSsl = true,
            UseDefaultCredentials = false,
        };
        var mailMessage = new MailMessage(_senderEmail, recipientEmail, subject, body);
        smtpClient.Send(mailMessage);
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