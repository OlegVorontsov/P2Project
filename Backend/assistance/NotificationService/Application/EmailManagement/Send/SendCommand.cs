namespace NotificationService.Application.EmailManagement.Send;

public record SendCommand(
    string RecipientEmail,
    string Subject,
    string Body);