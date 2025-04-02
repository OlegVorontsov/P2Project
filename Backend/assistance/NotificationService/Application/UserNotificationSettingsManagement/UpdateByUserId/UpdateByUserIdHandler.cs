using NotificationService.Dtos;
using NotificationService.Infrastructure.Repositories;

namespace NotificationService.Application.UserNotificationSettingsManagement.UpdateByUserId;

public class UpdateByUserIdHandler(NotificationRepository repository)
{
    public async Task Handle(
        Guid userId,
        SentNotificationSettings newNotificationSettings,
        CancellationToken ct)
    {
        await repository.Update(userId, newNotificationSettings, ct);
    }
}