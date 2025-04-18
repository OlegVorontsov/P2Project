using MassTransit;
using NotificationService.Application.EveryDestinationManagement.Send;
using NotificationService.Core.EmailMessages.Templates;
using P2Project.Accounts.Agreements.Messages;

namespace NotificationService.Infrastructure.Consumers;

public class ConfirmUserEmailConsumer(
    SendEveryDestinationHandler sendEveryDestinationHandler,
    ILogger<ConfirmUserEmailConsumer> logger)
    : IConsumer<ConfirmedUserEmailEvent>
{
    public async Task Consume(ConsumeContext<ConfirmedUserEmailEvent> context)
    {
        var command = context.Message;
        
        var sentResult = await sendEveryDestinationHandler.Handle(new SendEveryDestinationCommand(
            command.UserId,
            command.Email,
            EmailConfirmationEmailTemplate.Subject(),
            EmailConfirmationEmailTemplate.Body(command.UserName, command.EmailConfirmationLink),
            EmailConfirmationEmailTemplate.Styles(),
            $"Здравствуйте, {command.UserName}! Благодарим Вас за регистрацию на сайте P2Project. Для того чтобы завершить процесс регистрации, подтвердите Ваш email, перейдя по ссылке, отправленной на почту: {command.Email}"),
            CancellationToken.None);
        logger.LogInformation(sentResult);
    }
}