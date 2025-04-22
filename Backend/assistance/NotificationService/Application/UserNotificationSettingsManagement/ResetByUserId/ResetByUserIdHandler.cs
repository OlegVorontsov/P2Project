using NotificationService.Infrastructure;
using NotificationService.Infrastructure.Repositories;
using P2Project.Core.Interfaces.Commands;

namespace NotificationService.Application.UserNotificationSettingsManagement.ResetByUserId;

public class ResetByUserIdHandler(
    NotificationRepository repository,
    UnitOfWork unitOfWork) : ICommandVoidHandler<ResetByUserIdCommand>
{
    public async Task Handle(ResetByUserIdCommand command, CancellationToken ct)
    {
        var notificationSettingsExist = await repository.Get(command.UserId, ct);

        if (notificationSettingsExist is null) return;

        notificationSettingsExist.Edit(null, null, false);
        
        await unitOfWork.SaveChanges(ct);
    }
}