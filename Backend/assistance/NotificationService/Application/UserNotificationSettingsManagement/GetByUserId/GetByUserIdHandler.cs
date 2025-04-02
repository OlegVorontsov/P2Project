using NotificationService.Domain;
using NotificationService.Infrastructure.Repositories;

namespace NotificationService.Application.UserNotificationSettingsManagement.GetByUserId;

public class GetByUserIdHandler(NotificationRepository repository)
{
    public async Task<UserNotificationSettings?> Handle(Guid userId, CancellationToken ct)
    { 
        return await repository.Get(userId, ct);
    }
}