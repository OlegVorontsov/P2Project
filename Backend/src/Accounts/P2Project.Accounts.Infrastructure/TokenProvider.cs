using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using P2Project.Accounts.Application;
using P2Project.Accounts.Domain.User;

namespace P2Project.Accounts.Infrastructure;

public class TokenProvider : ITokenProvider
{
    public Task<string> GenerateAccessToken(User user)
    {
        var token = new JwtSecurityToken();
        
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, "userId"),
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("ajnbpiusrtoibahiutbheatpihgpeiaughpiauhgpitugha"));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            issuer: "test",
            audience: "test",
            claims: claims,
            signingCredentials: creds,
            expires: DateTime.UtcNow.AddMinutes(10));

        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
        return tokenString;
    }
}