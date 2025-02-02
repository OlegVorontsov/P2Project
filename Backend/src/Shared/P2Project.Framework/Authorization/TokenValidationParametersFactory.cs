using System.Text;
using Microsoft.IdentityModel.Tokens;
using P2Project.Core.Options;

namespace P2Project.Framework.Authorization;

public static class TokenValidationParametersFactory
{
    public static TokenValidationParameters CreateWithLifeTime(JwtOptions jwtOptions) =>
        new ()
        {
            ValidIssuer = jwtOptions.Issuer,
            ValidAudience = jwtOptions.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtOptions.Key)),
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ClockSkew = TimeSpan.Zero
        };
    
    public static TokenValidationParameters CreateWithoutLifeTime(JwtOptions jwtOptions) =>
        new ()
        {
            ValidIssuer = jwtOptions.Issuer,
            ValidAudience = jwtOptions.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtOptions.Key)),
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = false,
            ValidateIssuerSigningKey = true,
            ClockSkew = TimeSpan.Zero
        };
}