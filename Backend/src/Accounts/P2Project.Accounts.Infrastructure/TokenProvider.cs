using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using P2Project.Accounts.Application;
using P2Project.Accounts.Domain.User;

namespace P2Project.Accounts.Infrastructure;

public class TokenProvider : ITokenProvider
{
    private readonly JwtOptions _jwtOptions;
    public TokenProvider(IOptions<JwtOptions> jwtOptions)
    {
        _jwtOptions = jwtOptions.Value;
    }
    public string GenerateAccessToken(User user)
    {
        Guid jti = Guid.NewGuid();
        
        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_jwtOptions.Key));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, jti.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email?? string.Empty)
        };

        var token = new JwtSecurityToken(
            issuer: _jwtOptions.Issuer,
            audience: _jwtOptions.Audience,
            expires: DateTime.UtcNow.AddMinutes(_jwtOptions.ExpiredMinute),
            signingCredentials: creds,
            claims: claims);
        
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}