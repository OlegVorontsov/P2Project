using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using P2Project.Accounts.Infrastructure.Admin;
using P2Project.Accounts.Infrastructure.DbContexts;

namespace P2Project.Accounts.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddAccountsInfrastructure(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtOptions>(
            configuration.GetSection(JwtOptions.NAME));
        services.Configure<AdminOptions>(
            configuration.GetSection(AdminOptions.NAME));
        
        services.AddDataBase();
            //.AddUnitOfWork();
        
        return services;
    }
    
    private static IServiceCollection AddDataBase(
        this IServiceCollection services)
    {
        services.AddScoped<AuthorizationDbContext>();

        return services;
    }
}