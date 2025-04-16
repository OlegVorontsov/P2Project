namespace NotificationService.Core.Dtos;

public record SentNotificationSettings(
    string? Email,
    string? TelegramUserId,
    bool? IsWebSend);