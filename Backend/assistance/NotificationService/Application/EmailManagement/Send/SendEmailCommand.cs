using P2Project.Core.Interfaces.Commands;

namespace NotificationService.Application.EmailManagement.Send;

public record SendEmailCommand(
    string RecipientEmail,
    string Subject,
    string Body,
    string Styles) : ICommand;