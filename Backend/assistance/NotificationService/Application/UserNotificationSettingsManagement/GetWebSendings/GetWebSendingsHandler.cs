using NotificationService.Domain;
using NotificationService.Infrastructure.Repositories;

namespace NotificationService.Application.UserNotificationSettingsManagement.GetWebSendings;

public class GetWebSendingsHandler(NotificationRepository repository)
{
    public async Task<IReadOnlyList<UserNotificationSettings>> Handle(CancellationToken ct)
    { 
        return await repository.GetWebSendings(ct);
    }
}