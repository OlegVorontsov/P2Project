using NotificationService.Domain.ValueObjects;

namespace NotificationService.Domain;

public class UserNotificationSettings
{
    private UserNotificationSettings() { }
    
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public string? Email { get; private set; }
    public TelegramSettings? TelegramSettings { get; set; }
    public bool? IsWebSend { get; private set; } = false;
    
    private UserNotificationSettings(
        Guid userId,
        string? email,
        TelegramSettings? telegramSettings,
        bool? isWebSend)
    {
        UserId = userId;
        Email = email;
        TelegramSettings = telegramSettings;
        IsWebSend = isWebSend;
    }

    public static UserNotificationSettings Create(Guid userId, string? email, TelegramSettings? telegramSettings) =>
        new (userId, email, telegramSettings, true);

    public void Edit(string? email, TelegramSettings? telegramSettings, bool? isWebSend)
    {
        Email = email;
        TelegramSettings = telegramSettings;
        IsWebSend = isWebSend;
    }
    
    public void SetEmail(string? email) =>
        Email = email;
    
    public void SetTelegramSettings(TelegramSettings? telegramSettings) =>
        TelegramSettings = telegramSettings;
    
    public void SetWebSend(bool? isWebSend) =>
        IsWebSend = isWebSend;
}