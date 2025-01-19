using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using P2Project.Accounts.Domain.Role;
using P2Project.Accounts.Domain.User;
using P2Project.Accounts.Infrastructure.DbContexts;

namespace P2Project.Accounts.Web;

public static class DependencyInjection
{
    public static IServiceCollection AddAccountModule(
        this IServiceCollection services)
    {
        services.AddIdentity<User, Role>()
            .AddEntityFrameworkStores<AuthorizationDbContext>();
        
        services.AddScoped<AuthorizationDbContext>();
        
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

        services.AddAuthorization();

        return services;
    }
}