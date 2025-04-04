namespace NotificationService.Domain;

public class UserNotificationSettings
{
    private UserNotificationSettings() { }
    
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public bool? IsEmailSend { get; private set; } = false;
    public bool? IsTelegramSend { get; private set; } = false;
    public bool? IsWebSend { get; private set; } = false;
    
    private UserNotificationSettings(Guid userId)
    {
        UserId = userId;
    }

    public static UserNotificationSettings Create(Guid userId) => new (userId);
    
    public void Edit(bool? isEmailSend, bool? isTelegramSend, bool? isWebSend)
    {
        IsEmailSend = isEmailSend;
        IsTelegramSend = isTelegramSend;
        IsWebSend = isWebSend;
    }
}