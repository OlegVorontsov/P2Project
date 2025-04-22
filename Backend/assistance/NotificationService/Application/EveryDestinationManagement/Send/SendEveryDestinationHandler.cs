using NotificationService.Application.SendersManagement;
using NotificationService.Infrastructure.Repositories;
using P2Project.Core.Interfaces.Commands;

namespace NotificationService.Application.EveryDestinationManagement.Send;

public class SendEveryDestinationHandler(
    NotificationRepository repository,
    SendersFactory sendersFactory,
    ILogger<SendEveryDestinationHandler> logger) :
    ICommandResponseHandler<string, SendEveryDestinationCommand>
{
    public async Task<string> Handle(SendEveryDestinationCommand command, CancellationToken ct)
    {
        var notificationSettingsExist = await repository.Get(command.UserId, ct);
        
        var sentEveryDestinationResult = string.Empty;
        
        if (notificationSettingsExist is null)
        {
            sentEveryDestinationResult += "NotificationSettings is null";
            logger.LogError($"NotificationSettings is null user's: {command.UserId}");
            return sentEveryDestinationResult;
        }
        
        var senders = sendersFactory.GetSenders(notificationSettingsExist, ct);

        foreach (var sender in senders)
        {
            var sentResult = await sender.SendAsync(notificationSettingsExist, command, ct);
            if (sentResult.IsSuccess)
                sentEveryDestinationResult += sentResult.Value;
        }
        
        return sentEveryDestinationResult;
    }
}