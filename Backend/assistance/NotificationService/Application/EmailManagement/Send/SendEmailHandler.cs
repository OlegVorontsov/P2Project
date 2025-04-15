using CSharpFunctionalExtensions;
using NotificationService.Infrastructure.EmailNotification.EmailManagerImplementations;
using P2Project.Core.Interfaces.Commands;
using P2Project.SharedKernel.Errors;

namespace NotificationService.Application.EmailManagement.Send;

public class SendEmailHandler(IConfiguration configuration) :
    ICommandHandler<string, SendEmailCommand>
{
    public async Task<Result<string, ErrorList>> Handle(SendEmailCommand command, CancellationToken ct)
    {
        var emailManager = YandexEmailManager.Build(configuration);

        var sentResult = emailManager.SendMessage(
            command.RecipientEmail,
            command.Subject,
            command.Body);
        if (sentResult.IsFailure)
            return Errors.General.Failure(sentResult.Error.Message).ToErrorList();

        return "Email sent successfully";
    }
}