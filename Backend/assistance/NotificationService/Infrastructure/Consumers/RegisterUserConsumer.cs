using MassTransit;
using P2Project.Accounts.Agreements.Messages;

namespace NotificationService.Infrastructure.Consumers;

public class RegisterUserConsumer : IConsumer<CreatedUserEvent>
{
    public async Task Consume(
        ConsumeContext<CreatedUserEvent> context)
    {
        throw new NotImplementedException();
    }
}