namespace NotificationService.Domain;

public class UserNotificationSettings
{
    private UserNotificationSettings() { }
    
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public bool? IsEmailSend { get; private set; } = false;
    public bool? IsTelegramSend { get; private set; } = false;
    public bool? IsWebSend { get; private set; } = false;
}