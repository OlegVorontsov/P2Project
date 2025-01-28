using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using P2Project.Accounts.Application;
using P2Project.Accounts.Domain;
using P2Project.Accounts.Infrastructure.Managers;
using P2Project.Accounts.Infrastructure.Models;

namespace P2Project.Accounts.Infrastructure.Jwt;

public class TokenProvider : ITokenProvider
{
    private readonly PermissionManager _permissionManager;
    private readonly JwtOptions _jwtOptions;
    public TokenProvider(
        IOptions<JwtOptions> jwtOptions,
        PermissionManager permissionManager)
    {
        _permissionManager = permissionManager;
        _jwtOptions = jwtOptions.Value;
    }
    public async Task<string> GenerateAccessToken(User user)
    {
        Guid jti = Guid.NewGuid();
        
        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_jwtOptions.Key));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var roleClaims = user.Roles
            .Select(r => new Claim(CustomClaims.ROLE, r.Name ?? string.Empty));
        
        var permissions = await _permissionManager.GetUserPermissions(user.Id);
        var permissionClaims = permissions.Select(p => new Claim(CustomClaims.PERMISSION, p));

        Claim[] claims = [
            //new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            //new Claim(JwtRegisteredClaimNames.Jti, jti.ToString()),
            //new Claim(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty)
            new Claim(CustomClaims.Id, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty)
        ];
        
        claims = claims.Concat(roleClaims).Concat(permissionClaims).ToArray();

        var token = new JwtSecurityToken(
            issuer: _jwtOptions.Issuer,
            audience: _jwtOptions.Audience,
            expires: DateTime.UtcNow.AddMinutes(_jwtOptions.ExpiredMinute),
            signingCredentials: creds,
            claims: claims);
        
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}