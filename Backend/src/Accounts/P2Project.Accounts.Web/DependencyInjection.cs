using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using P2Project.Accounts.Application;
using P2Project.Accounts.Domain.Role;
using P2Project.Accounts.Domain.User;
using P2Project.Accounts.Infrastructure;
using P2Project.Accounts.Infrastructure.DbContexts;

namespace P2Project.Accounts.Web;

public static class DependencyInjection
{
    public static IServiceCollection AddAccountsModule(
        this IServiceCollection services, IConfiguration configuration)
    {
        return services.AddTransient<ITokenProvider, TokenProvider>()
                       .AddUsersIdentity()
                       .AddAccountsInfrastructure(configuration)
                       .AddUsersAuthentication(configuration)
                       .AddAuthorization()
                       .AddAccountsApplication();
    }

    private static IServiceCollection AddUsersIdentity(
        this IServiceCollection services)
    {
        services.AddIdentity<User, Role>(options =>
            {
                options.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<AuthorizationDbContext>()
            .AddDefaultTokenProviders();
        
        return services;
    }

    private static IServiceCollection AddUsersAuthentication(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                var jwtOptions = configuration
                                     .GetSection(JwtOptions.NAME).Get<JwtOptions>() ??
                                 throw new ApplicationException("Jwt configuration missed");
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidIssuer = jwtOptions.Issuer,
                    ValidAudience = jwtOptions.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwtOptions.Key)),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                };
            });
        return services;
    }
}