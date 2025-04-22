using NotificationService.Domain;
using NotificationService.Infrastructure.Repositories;
using P2Project.Core.Interfaces.Queries;

namespace NotificationService.Application.UserNotificationSettingsManagement.GetTelegramSendings;

public class GetTelegramSendingsHandler(NotificationRepository repository) :
    IQueryHandler<IReadOnlyList<UserNotificationSettings>>
{
    public async Task<IReadOnlyList<UserNotificationSettings>> Handle(CancellationToken ct)
    {
        return await repository.GetTelegramSendings(ct);
    }
}