using CSharpFunctionalExtensions;
using NotificationService.Core.Dtos;
using NotificationService.Domain;
using NotificationService.Infrastructure;
using NotificationService.Infrastructure.Repositories;
using NotificationService.Infrastructure.TelegramNotification;
using P2Project.SharedKernel.Errors;

namespace NotificationService.Application.UserNotificationSettingsManagement.SetByUserId;

public class SetByUserIdHandler(
    NotificationRepository repository,
    TelegramManager telegramManager,
    UnitOfWork unitOfWork)
{
    public async Task<Result<UserNotificationSettings, ErrorList>> Handle(
        Guid userId,
        SentNotificationSettings notificationSettings,
        CancellationToken ct)
    {
        var notificationSettingsExist = await repository.Get(userId, ct);
        
        if (notificationSettingsExist is null)
        {
            var newUserNotificationSettings = UserNotificationSettings.Create(userId, null);
            await repository.Add(newUserNotificationSettings, ct);
            await unitOfWork.SaveChanges(ct);
            
            if (notificationSettings.TelegramUserId == null)
                return newUserNotificationSettings;
            
            var registerResult = await telegramManager.StartRegisterChatId(
                userId, notificationSettings.TelegramUserId);
            if (registerResult.IsFailure)
                return Errors.General.Failure("Fail to register telegramChatId").ToErrorList();
            return registerResult.Value;
        }

        if (notificationSettingsExist.TelegramSettings != null)
        {
            if (notificationSettingsExist.TelegramSettings.UserId == notificationSettings.TelegramUserId)
            {
                notificationSettingsExist.SetEmailSend(notificationSettings.IsEmailSend);
                notificationSettingsExist.SetWebSend(notificationSettings.IsWebSend);
                await unitOfWork.SaveChanges(ct);
                return notificationSettingsExist;
            }
            if (notificationSettings.TelegramUserId != null)
                return Errors.General.Failure("Can't change TelegramUserId").ToErrorList();
        }
        
        if (notificationSettings.TelegramUserId != null)
        {
            var registerResult = await telegramManager.StartRegisterChatId(
                userId, notificationSettings.TelegramUserId);
            if (registerResult.IsFailure)
                return Errors.General.Failure("Fail to register telegramChatId").ToErrorList();
            
            notificationSettingsExist.SetEmailSend(notificationSettings.IsEmailSend);
            notificationSettingsExist.SetWebSend(notificationSettings.IsWebSend);
            await unitOfWork.SaveChanges(ct);
            return registerResult.Value;
        }

        notificationSettingsExist.Edit(
            notificationSettings.IsEmailSend,
            null,
            notificationSettings.IsWebSend);

        await unitOfWork.SaveChanges(ct);
        
        return notificationSettingsExist;
    }
}