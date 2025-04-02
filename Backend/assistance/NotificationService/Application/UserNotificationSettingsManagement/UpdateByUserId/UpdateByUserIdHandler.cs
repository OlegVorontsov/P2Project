using NotificationService.Core.Dtos;
using NotificationService.Infrastructure;
using NotificationService.Infrastructure.Repositories;

namespace NotificationService.Application.UserNotificationSettingsManagement.UpdateByUserId;

public class UpdateByUserIdHandler(
    NotificationRepository repository,
    UnitOfWork unitOfWork)
{
    public async Task Handle(
        Guid userId,
        SentNotificationSettings newNotificationSettings,
        CancellationToken ct)
    {
        await repository.Update(userId, newNotificationSettings, ct);
        await unitOfWork.SaveChanges(ct);
    }
}