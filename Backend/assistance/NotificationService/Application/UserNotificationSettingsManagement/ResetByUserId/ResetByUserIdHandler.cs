using NotificationService.Infrastructure;
using NotificationService.Infrastructure.Repositories;

namespace NotificationService.Application.UserNotificationSettingsManagement.ResetByUserId;

public class ResetByUserIdHandler(
    NotificationRepository repository,
    UnitOfWork unitOfWork)
{
    public async Task Handle(Guid userId, CancellationToken ct)
    {
        var notificationSettingsExist = await repository.Get(userId, ct);

        if (notificationSettingsExist is null) return;

        notificationSettingsExist.Edit(false, false, false);
        
        await unitOfWork.SaveChanges(ct);
    }
}