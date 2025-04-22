using CSharpFunctionalExtensions;
using NotificationService.Application.EveryDestinationManagement.Send;
using NotificationService.Application.Interfaces;
using NotificationService.Domain;
using NotificationService.Infrastructure.EmailNotification.EmailManagerImplementations;
using P2Project.SharedKernel.Errors;

namespace NotificationService.Application.SendersManagement.Senders;

public class EmailNotificationSender(IConfiguration configuration) : INotificationSender
{
    public async Task<Result<string, ErrorList>> SendAsync(
        SendEveryDestinationCommand command, CancellationToken ct)
    {
        var emailManager = YandexEmailManager.Build(configuration);

        var sentResult = emailManager.SendMessage(
            command.Email,
            command.Subject,
            command.Body);
        if (sentResult.IsFailure)
            return Errors.General.Failure(sentResult.Error.Message).ToErrorList();

        return $"Email sent successfully to: {command.Email}";
    }
    
    public bool CanSend(
        UserNotificationSettings userNotificationSetting,
        CancellationToken cancellationToken) => userNotificationSetting.Email != null;
}