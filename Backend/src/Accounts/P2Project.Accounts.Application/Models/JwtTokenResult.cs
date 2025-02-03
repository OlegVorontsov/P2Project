namespace P2Project.Accounts.Application.Models;

public record JwtTokenResult(string AccessToken, Guid Jti);