namespace NotificationService.Domain;

public class UserNotificationSettings
{
    private UserNotificationSettings() { }
    
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public bool? IsEmailSend { get; private set; } = false;
    public bool? IsTelegramSend { get; private set; } = false;
    public long? TelegramChatId { get; set; }
    public bool? IsWebSend { get; private set; } = false;
    
    private UserNotificationSettings(Guid userId, bool? isTelegramSend, long? telegramChatId)
    {
        UserId = userId;
        IsTelegramSend = isTelegramSend;
        TelegramChatId = telegramChatId;
    }

    public static UserNotificationSettings Create(Guid userId, bool? isTelegramSend, long? telegramChatId) =>
        new (userId, isTelegramSend, telegramChatId);

    public void Edit(bool? isEmailSend, bool? isTelegramSend, long? telegramChatId, bool? isWebSend)
    {
        IsEmailSend = isEmailSend;
        IsTelegramSend = isTelegramSend;
        TelegramChatId = telegramChatId;
        IsWebSend = isWebSend;
    }
    
    public void SetTelegramChatId(long? telegramChatId) => TelegramChatId = telegramChatId;
}