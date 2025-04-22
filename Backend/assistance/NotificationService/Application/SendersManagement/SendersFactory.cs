using NotificationService.Application.Interfaces;
using NotificationService.Domain;

namespace NotificationService.Application.SendersManagement;

public class SendersFactory
{
    private readonly IEnumerable<INotificationSender> _senders;

    public SendersFactory(IEnumerable<INotificationSender> senders)
    {
        _senders = senders.ToList();
    }

    public IEnumerable<INotificationSender> GetSenders(
        UserNotificationSettings userNotificationSetting,
        CancellationToken cancellationToken) => _senders.Where(sender =>
        sender.CanSend(userNotificationSetting, cancellationToken));
}