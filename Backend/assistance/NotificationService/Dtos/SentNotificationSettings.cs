namespace NotificationService.Dtos;

public record SentNotificationSettings(bool? IsEmailSend, bool? IsTelegramSend, bool? IsWebSend);