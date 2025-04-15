using NotificationService.Core.Dtos;
using P2Project.Core.Interfaces.Commands;

namespace NotificationService.Application.UserNotificationSettingsManagement.SetByUserId;

public record SetByUserIdCommand(
    Guid UserId,
    SentNotificationSettings NotificationSettings) : ICommand;