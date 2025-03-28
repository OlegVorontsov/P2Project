using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using P2Project.Accounts.Agreements;
using P2Project.Accounts.Application;
using P2Project.Accounts.Application.Interfaces;
using P2Project.Accounts.Domain;
using P2Project.Accounts.Domain.RolePermission.Roles;
using P2Project.Accounts.Infrastructure;
using P2Project.Accounts.Infrastructure.DbContexts;
using P2Project.Accounts.Infrastructure.Jwt;
using P2Project.Accounts.Infrastructure.Managers;
using P2Project.Accounts.Infrastructure.Seedings;
using P2Project.Core.Options;
using P2Project.Framework.Authorization;

namespace P2Project.Accounts.Web;

public static class DependencyInjection
{
    public static IServiceCollection AddAccountsModule(
        this IServiceCollection services, IConfiguration configuration)
    {
        return services.AddTransient<ITokenProvider, TokenProvider>()
            .AddScoped<IAccountsAgreements, AccountsAgreements>()
            .AddAccountsInfrastructure(configuration)
            .AddUsersIdentity()
            .AddUsersAuthentication(configuration)
            .AddUsersAuthorization()
            .AddHttpContextAccessor().AddScoped<UserScopedData>()
            .AddAccountsApplication()
            .AddSeedings();
    }

    private static IServiceCollection AddUsersIdentity(
        this IServiceCollection services)
    {
        services.AddIdentity<User, Role>(options => { options.User.RequireUniqueEmail = true; })
            .AddEntityFrameworkStores<AccountsWriteDbContext>()
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
                
                options.TokenValidationParameters = TokenValidationParametersFactory.CreateWithLifeTime(jwtOptions);
            });
        return services;
    }

    private static IServiceCollection AddUsersAuthorization(
        this IServiceCollection services)
    {
        services.AddAuthorization();
        
        services.AddSingleton<IAuthorizationHandler, PermissionRequirementHandler>();
        services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();

        return services;
    }

    private static IServiceCollection AddSeedings(
        this IServiceCollection services)
    {
        services.AddScoped<PermissionManager>()
                .AddScoped<RolePermissionManager>()
                .AddScoped<IAccountsManager, AccountsManager>()
                .AddScoped<IRefreshSessionManager, RefreshSessionManager>()
                .AddSingleton<AccountSeeder>()
                .AddScoped<AccountsSeederService>();

        return services;
    }
}