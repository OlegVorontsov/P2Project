using P2Project.Core.Interfaces.Queries;

namespace NotificationService.Application.UserNotificationSettingsManagement.GetByUserId;

public record GetByUserIdQuery(Guid UserId) : IQuery;