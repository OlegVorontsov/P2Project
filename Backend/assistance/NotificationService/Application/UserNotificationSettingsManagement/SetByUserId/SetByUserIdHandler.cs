using NotificationService.Core.Dtos;
using NotificationService.Domain;
using NotificationService.Infrastructure;
using NotificationService.Infrastructure.Repositories;

namespace NotificationService.Application.UserNotificationSettingsManagement.SetByUserId;

public class SetByUserIdHandler(
    NotificationRepository repository,
    UnitOfWork unitOfWork)
{
    public async Task<UserNotificationSettings> Handle(
        Guid userId,
        SentNotificationSettings newNotificationSettings,
        CancellationToken ct)
    {
        var result = await repository.Set(userId, newNotificationSettings, ct);
        await unitOfWork.SaveChanges(ct);
        return result;
    }
}