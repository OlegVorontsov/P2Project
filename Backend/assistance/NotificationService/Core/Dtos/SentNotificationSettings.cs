namespace NotificationService.Core.Dtos;

public record SentNotificationSettings(
    bool? IsEmailSend,
    bool? IsTelegramSend,
    long? TelegramChatId,
    bool? IsWebSend);