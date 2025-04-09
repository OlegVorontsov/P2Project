namespace NotificationService.Application.EventHandlers;

public class ConfirmationEmailHandler
{
    public async Task Handle(Guid userId, CancellationToken ct)
    {
        using var httpClient = new HttpClient();
        var apiUrl = "http://localhost:5190";
        
        var confirmationEmailUrl = $"{apiUrl}/api/Accounts/confirmation-email/token/{userId}";
        using var confirmationEmailMessage = new HttpRequestMessage(HttpMethod.Get, confirmationEmailUrl);
        
        using var response = await httpClient.SendAsync(confirmationEmailMessage);
        response.EnsureSuccessStatusCode();
    }
}