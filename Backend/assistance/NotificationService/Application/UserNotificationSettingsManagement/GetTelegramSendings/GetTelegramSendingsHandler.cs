using NotificationService.Domain;
using NotificationService.Infrastructure.Repositories;

namespace NotificationService.Application.UserNotificationSettingsManagement.GetTelegramSendings;

public class GetTelegramSendingsHandler(NotificationRepository repository)
{
    public async Task<IReadOnlyList<UserNotificationSettings>> Handle(CancellationToken ct)
    {
        return await repository.GetTelegramSendings(ct);
    }
}