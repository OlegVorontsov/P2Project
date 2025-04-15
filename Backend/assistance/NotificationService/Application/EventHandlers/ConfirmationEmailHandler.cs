using P2Project.Core.Interfaces.Commands;

namespace NotificationService.Application.EventHandlers;

public class ConfirmationEmailHandler :
    ICommandVoidHandler<ConfirmationEmailCommand>
{
    public async Task Handle(ConfirmationEmailCommand command, CancellationToken ct)
    {
        using var httpClient = new HttpClient();
        var apiUrl = "http://localhost:5190";
        
        var confirmationEmailUrl = $"{apiUrl}/api/Accounts/confirmation-email/token/{command.UserId}";
        using var confirmationEmailMessage = new HttpRequestMessage(HttpMethod.Get, confirmationEmailUrl);
        
        using var response = await httpClient.SendAsync(confirmationEmailMessage);
        response.EnsureSuccessStatusCode();
    }
}