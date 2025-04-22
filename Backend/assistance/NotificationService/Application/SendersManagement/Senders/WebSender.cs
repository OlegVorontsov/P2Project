using CSharpFunctionalExtensions;
using NotificationService.Application.EveryDestinationManagement.Send;
using NotificationService.Application.Interfaces;
using NotificationService.Domain;
using P2Project.SharedKernel.Errors;

namespace NotificationService.Application.SendersManagement.Senders;

public class WebSender : INotificationSender
{
    public async Task<Result<string, ErrorList>> SendAsync(
        UserNotificationSettings userNotificationSetting,
        SendEveryDestinationCommand command,
        CancellationToken ct)
    {
        return "Web message sent successfully\n";
    }
    
    public bool CanSend(
        UserNotificationSettings userNotificationSetting,
        CancellationToken cancellationToken) =>
        userNotificationSetting.IsWebSend.HasValue &&
        userNotificationSetting.IsWebSend.Value;
}