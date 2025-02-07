using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using P2Project.Accounts.Application.Interfaces;
using P2Project.Accounts.Infrastructure.Admin;
using P2Project.Accounts.Infrastructure.DbContexts;
using P2Project.Core;
using P2Project.Core.Interfaces;
using P2Project.Core.Options;

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
        
        services.AddDataBase()
                .AddUnitOfWork();
        
        return services;
    }
    
    private static IServiceCollection AddDataBase(
        this IServiceCollection services)
    {
        services.AddScoped<AccountsWriteDbContext>();
        services.AddScoped<IAccountsReadDbContext, AccountsReadDbContext>();

        return services;
    }
    
    private static IServiceCollection AddUnitOfWork(
        this IServiceCollection services)
    {
        services.AddKeyedScoped<IUnitOfWork, UnitOfWork>(Modules.Accounts);

        return services;
    }
}