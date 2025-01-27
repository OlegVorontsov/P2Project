using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using P2Project.Accounts.Application;
using P2Project.Accounts.Domain;
using P2Project.Accounts.Domain.RolePermission.Roles;
using P2Project.Accounts.Infrastructure;
using P2Project.Accounts.Infrastructure.DbContexts;
using P2Project.Accounts.Infrastructure.Jwt;
using P2Project.Accounts.Infrastructure.Managers;
using P2Project.Accounts.Infrastructure.Permissions;
using P2Project.Accounts.Infrastructure.Seedings;

namespace P2Project.Accounts.Web;

public static class DependencyInjection
{
    public static IServiceCollection AddAccountsModule(
        this IServiceCollection services, IConfiguration configuration)
    {
        return services.AddTransient<ITokenProvider, TokenProvider>()
                       .AddAccountsInfrastructure(configuration)
                       .AddUsersIdentity()
                       .AddUsersAuthentication(configuration)
                       .AddUsersAuthorization()
                       .AddAccountsApplication()
                       .AddSeedings();
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
                    ClockSkew = TimeSpan.Zero
                };
            });
        return services;
    }
    
    private static IServiceCollection AddUsersAuthorization(
        this IServiceCollection services)
    {
        services.AddAuthorization();
        
        services.AddSingleton<IAuthorizationHandler, PermissionAttributeHandler>();
        services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();
        
        return services;
    }
    
    private static IServiceCollection AddSeedings(
        this IServiceCollection services)
    {
        //services.SeedRolesWithPermissions();
        //services.SeedAdmins(configuration);
        services.AddSingleton<AccountSeeder>()
            .AddScoped<AccountsSeederService>()
            .AddScoped<PermissionManager>()
            .AddScoped<RolePermissionManager>()
            .AddScoped<AdminAccountManager>();

        return services;
    }
}