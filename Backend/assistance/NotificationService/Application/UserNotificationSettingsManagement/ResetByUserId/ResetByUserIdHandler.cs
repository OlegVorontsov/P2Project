using NotificationService.Infrastructure;
using NotificationService.Infrastructure.Repositories;

namespace NotificationService.Application.UserNotificationSettingsManagement.ResetByUserId;

public class ResetByUserIdHandler(
    NotificationRepository repository,
    UnitOfWork unitOfWork)
{
    public async Task Handle(Guid userId, CancellationToken ct)
    {
        await repository.Reset(userId, ct);
        await unitOfWork.SaveChanges(ct);
    }
}