namespace NotificationService.Core.Dtos;

public record SentNotificationSettings(bool? IsEmailSend, bool? IsTelegramSend, bool? IsWebSend);