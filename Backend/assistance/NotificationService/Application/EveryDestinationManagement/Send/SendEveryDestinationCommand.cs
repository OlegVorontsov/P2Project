namespace NotificationService.Application.EveryDestinationManagement.Send;

public record SendEveryDestinationCommand(
    Guid UserId,
    string Email,
    string Subject,
    string Body,
    string Styles);