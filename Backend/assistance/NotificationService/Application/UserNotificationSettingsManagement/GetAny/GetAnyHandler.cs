using NotificationService.Domain;
using NotificationService.Infrastructure.Repositories;
using P2Project.Core.Interfaces.Queries;

namespace NotificationService.Application.UserNotificationSettingsManagement.GetAny;

public class GetAnyHandler(NotificationRepository repository)
    : IQueryHandler<IReadOnlyList<UserNotificationSettings>>
{
    public async Task<IReadOnlyList<UserNotificationSettings>> Handle(CancellationToken ct)
    {
        return await repository.GetAnySending(ct);
    }
}