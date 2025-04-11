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
            return notificationSettings.TelegramChatId.HasValue ?
                UserNotificationSettings.Create(userId, true, notificationSettings.TelegramChatId) :
                UserNotificationSettings.Create(userId, false, null);

        notificationSettingsExist.Edit(
            notificationSettings.IsEmailSend,
            notificationSettings.TelegramChatId.HasValue,
            notificationSettings.TelegramChatId,
            notificationSettings.IsWebSend);

        await unitOfWork.SaveChanges(ct);
        
        return notificationSettingsExist;
    }
}