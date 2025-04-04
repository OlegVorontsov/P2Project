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
        SentNotificationSettings notificationSettings,
        CancellationToken ct)
    {
        var notificationSettingsExist = await repository.Get(userId, ct);
        if (notificationSettingsExist is null)
            return UserNotificationSettings.Create(userId);
        
        notificationSettingsExist.Edit(
            notificationSettings.IsEmailSend,
            notificationSettings.IsTelegramSend,
            notificationSettings.IsWebSend);
        await unitOfWork.SaveChanges(ct);
        
        return notificationSettingsExist;
    }
}