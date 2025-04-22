using P2Project.Core.Interfaces.Commands;

namespace NotificationService.Application.UserNotificationSettingsManagement.ResetByUserId;

public record ResetByUserIdCommand(Guid UserId) : ICommand;