using NotificationService.Domain;
using NotificationService.Infrastructure.Repositories;
using P2Project.Core.Interfaces.Queries;

namespace NotificationService.Application.UserNotificationSettingsManagement.GetByUserId;

public class GetByUserIdHandler(NotificationRepository repository) :
    IQueryHandler<UserNotificationSettings?, GetByUserIdQuery>
{
    public async Task<UserNotificationSettings?> Handle(GetByUserIdQuery query, CancellationToken ct)
    { 
        return await repository.Get(query.UserId, ct);
    }
}