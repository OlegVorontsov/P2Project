using NotificationService.Domain.ValueObjects;

namespace NotificationService.Domain;

public class UserNotificationSettings
{
    private UserNotificationSettings() { }
    
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public bool? IsEmailSend { get; private set; } = false;
    public TelegramSettings? TelegramSettings { get; set; }
    public bool? IsWebSend { get; private set; } = false;
    
    private UserNotificationSettings(
        Guid userId,
        bool? isEmailSend,
        TelegramSettings? telegramSettings,
        bool? isWebSend)
    {
        UserId = userId;
        IsEmailSend = isEmailSend;
        TelegramSettings = telegramSettings;
        IsWebSend = isWebSend;
    }

    public static UserNotificationSettings Create(Guid userId, TelegramSettings? telegramSettings) =>
        new (userId, true, telegramSettings, true);

    public void Edit(bool? isEmailSend, TelegramSettings? telegramSettings, bool? isWebSend)
    {
        IsEmailSend = isEmailSend;
        TelegramSettings = telegramSettings;
        IsWebSend = isWebSend;
    }
    
    public void SetEmailSend(bool? isEmailSend) =>
        IsEmailSend = isEmailSend;
    
    public void SetTelegramSettings(TelegramSettings? telegramSettings) =>
        TelegramSettings = telegramSettings;
    
    public void SetWebSend(bool? isWebSend) =>
        IsWebSend = isWebSend;
}