namespace NotificationService.Core.Dtos;

public record SentNotificationSettings(
    bool? IsEmailSend,
    string? TelegramUserId,
    bool? IsWebSend);