using NotificationService.Domain;
using NotificationService.Infrastructure.Repositories;

namespace NotificationService.Application.UserNotificationSettingsManagement.GetEmailSendings;

public class GetEmailSendingsHandler(NotificationRepository repository)
{
    public async Task<IReadOnlyList<UserNotificationSettings>> Handle(CancellationToken ct)
    {
        return await repository.GetEmailSendings(ct);
    }
}