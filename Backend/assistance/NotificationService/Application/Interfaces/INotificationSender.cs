using CSharpFunctionalExtensions;
using NotificationService.Application.EveryDestinationManagement.Send;
using NotificationService.Domain;
using P2Project.SharedKernel.Errors;

namespace NotificationService.Application.Interfaces;

public interface INotificationSender
{
    Task <Result<string, ErrorList>>SendAsync(SendEveryDestinationCommand command, CancellationToken cancellationToken);
    public bool CanSend(UserNotificationSettings userNotificationSetting, CancellationToken cancellationToken);
}