using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CSharpFunctionalExtensions;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using P2Project.Accounts.Application.Interfaces;
using P2Project.Accounts.Application.Models;
using P2Project.Accounts.Domain;
using P2Project.Accounts.Infrastructure.DbContexts;
using P2Project.Accounts.Infrastructure.Managers;
using P2Project.Core.Options;
using P2Project.Framework.Authorization;
using P2Project.SharedKernel.Errors;

namespace P2Project.Accounts.Infrastructure.Jwt;

public class TokenProvider : ITokenProvider
{
    private readonly AuthorizationDbContext _dbContext;
    private readonly PermissionManager _permissionManager;
    private readonly JwtOptions _jwtOptions;
    public TokenProvider(
        AuthorizationDbContext dbContext,
        IOptions<JwtOptions> jwtOptions,
        PermissionManager permissionManager)
    {
        _dbContext = dbContext;
        _permissionManager = permissionManager;
        _jwtOptions = jwtOptions.Value;
    }
    public async Task<JwtTokenResult> GenerateAccessToken(
        User user, CancellationToken cancellationToken)
    {
        Guid jti = Guid.NewGuid();
        
        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_jwtOptions.Key));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var roleClaims = user.Roles
            .Select(r => new Claim(CustomClaims.ROLE, r.Name ?? string.Empty));
        
        var permissions = await _permissionManager
            .GetUserPermissions(user.Id, cancellationToken);
        var permissionClaims = permissions.Select(p => new Claim(CustomClaims.PERMISSION, p));

        Claim[] claims = [
            //new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            //new Claim(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty)
            new (CustomClaims.Id, user.Id.ToString()),
            new (CustomClaims.Jti, jti.ToString()),
            new (CustomClaims.Email, user.Email ?? string.Empty)
        ];
        
        claims = claims.Concat(roleClaims).Concat(permissionClaims).ToArray();

        var token = new JwtSecurityToken(
            issuer: _jwtOptions.Issuer,
            audience: _jwtOptions.Audience,
            expires: DateTime.UtcNow.AddMinutes(_jwtOptions.ExpiredMinute),
            signingCredentials: creds,
            claims: claims);
        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
        
        return new JwtTokenResult(tokenString, jti);
    }

    public async Task<Guid> GenerateRefreshToken(
        User user,
        Guid accessTokenJti,
        CancellationToken cancellationToken)
    {
        var refreshSession = new RefreshSession
        {
            User = user,
            Jti = accessTokenJti,
            CreatedAt = DateTime.UtcNow,
            ExpiresIn = DateTime.UtcNow.AddDays(60),
            RefreshToken = Guid.NewGuid()
        };
        _dbContext.RefreshSessions.Add(refreshSession);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return refreshSession.RefreshToken;
    }
    
    public async Task<Result<IReadOnlyList<Claim>, Error>> GetUserClaims(
        string jwtToken, CancellationToken cancellationToken)
    {
        var jwtHandler = new JwtSecurityTokenHandler();
        var validationParameters = TokenValidationParametersFactory.CreateWithoutLifeTime(_jwtOptions);

        var validationResult = await jwtHandler.ValidateTokenAsync(jwtToken, validationParameters);
        if (validationResult.IsValid == false)
            return Errors.AccountError.InvalidToken();
        
        return validationResult.ClaimsIdentity.Claims.ToList();
    }
}