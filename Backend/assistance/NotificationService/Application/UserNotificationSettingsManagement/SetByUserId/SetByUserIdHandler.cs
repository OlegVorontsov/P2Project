using CSharpFunctionalExtensions;
using NotificationService.Domain;
using NotificationService.Infrastructure;
using NotificationService.Infrastructure.Repositories;
using NotificationService.Infrastructure.TelegramNotification;
using P2Project.Core.Interfaces.Commands;
using P2Project.SharedKernel.Errors;

namespace NotificationService.Application.UserNotificationSettingsManagement.SetByUserId;

public class SetByUserIdHandler(
    NotificationRepository repository,
    TelegramManager telegramManager,
    UnitOfWork unitOfWork) :
    ICommandHandler<UserNotificationSettings, SetByUserIdCommand>
{
    public async Task<Result<UserNotificationSettings, ErrorList>> Handle(
        SetByUserIdCommand command,
        CancellationToken ct)
    {
        var notificationSettingsExist = await repository.Get(command.UserId, ct);
        
        if (notificationSettingsExist is null)
        {
            var newUserNotificationSettings = UserNotificationSettings.Create(
                command.UserId, command.NotificationSettings.Email, null);
            
            await repository.Add(newUserNotificationSettings, ct);
            await unitOfWork.SaveChanges(ct);
            
            if (command.NotificationSettings.TelegramUserId == null)
                return newUserNotificationSettings;
            
            var registerResult = await telegramManager.StartRegisterChatId(
                command.UserId,
                command.NotificationSettings.TelegramUserId);
            if (registerResult.IsFailure)
                return Errors.General.Failure("Fail to register telegramChatId").ToErrorList();
            return registerResult.Value;
        }

        if (notificationSettingsExist.TelegramSettings != null)
        {
            if (notificationSettingsExist.TelegramSettings.UserId == command.NotificationSettings.TelegramUserId)
            {
                notificationSettingsExist.SetEmail(command.NotificationSettings.Email);
                notificationSettingsExist.SetWebSend(command.NotificationSettings.IsWebSend);
                await unitOfWork.SaveChanges(ct);
                return notificationSettingsExist;
            }
            if (command.NotificationSettings.TelegramUserId != null)
                return Errors.General.Failure("Can't change TelegramUserId").ToErrorList();
        }
        
        if (command.NotificationSettings.TelegramUserId != null)
        {
            notificationSettingsExist.SetEmail(command.NotificationSettings.Email);
            notificationSettingsExist.SetWebSend(command.NotificationSettings.IsWebSend);
            var registerResult = await telegramManager.StartRegisterChatId(
                command.UserId,
                command.NotificationSettings.TelegramUserId);
            if (registerResult.IsFailure)
                return Errors.General.Failure("Fail to register telegramChatId").ToErrorList();

            return registerResult.Value;
        }

        notificationSettingsExist.Edit(
            command.NotificationSettings.Email,
            null,
            command.NotificationSettings.IsWebSend);

        await unitOfWork.SaveChanges(ct);
        
        return notificationSettingsExist;
    }
}