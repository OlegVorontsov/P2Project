namespace P2Project.Accounts.Agreements.Responses;

public record LoginResponse(string AccessToken, Guid RefreshToken);