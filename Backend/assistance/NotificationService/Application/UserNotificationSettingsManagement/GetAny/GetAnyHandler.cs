using NotificationService.Domain;
using NotificationService.Infrastructure.Repositories;

namespace NotificationService.Application.UserNotificationSettingsManagement.GetAny;

public class GetAnyHandler(NotificationRepository repository)
{
    public async Task<IReadOnlyList<UserNotificationSettings>> Handle(CancellationToken ct)
    {
        return await repository.GetAnySending(ct);
    }
}