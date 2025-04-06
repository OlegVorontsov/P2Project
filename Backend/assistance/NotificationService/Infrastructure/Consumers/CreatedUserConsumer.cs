using MassTransit;
using NotificationService.Infrastructure.EmailNotification.EmailManagerImplementations;
using P2Project.Accounts.Agreements.Messages;

namespace NotificationService.Infrastructure.Consumers;

public class CreatedUserConsumer(IConfiguration configuration)
    : IConsumer<CreatedUserEvent>
{
    public async Task Consume(
        ConsumeContext<CreatedUserEvent> context)
    {
        var command = context.Message;
        var emailManager = YandexEmailManager.Build(configuration);
        emailManager.SendMessage(
            command.Email,
            "Регистрация",
            $"Добро пожаловать, {command.UserName}! Для подтверждения почты перейдите по ссылке {command.UserId}.com"); 
        return;
    }
}