using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
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
        return services.AddUsersIdentity()
                       .AddAccountsInfrastructure(configuration)
                       .AddUsersAuthentication()
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
            .AddEntityFrameworkStores<AuthorizationDbContext>();
        
        return services;
    }

    private static IServiceCollection AddUsersAuthentication(
        this IServiceCollection services)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidIssuer = "test",
                    ValidAudience = "test",
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes("ajnbpiusrtoibahiutbheatpihgpeiaughpiauhgpitugha")),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = false,
                    ValidateIssuerSigningKey = true,
                };
            });
        return services;
    }
}