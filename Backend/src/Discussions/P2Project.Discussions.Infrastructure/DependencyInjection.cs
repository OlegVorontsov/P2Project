using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using P2Project.Core;
using P2Project.Core.Interfaces;
using P2Project.Discussions.Application;
using P2Project.Discussions.Application.Interfaces;
using P2Project.Discussions.Infrastructure.DbContexts;
using P2Project.SharedKernel;

namespace P2Project.Discussions.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddDiscussionsInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddRepositories()
            .AddDataBase(configuration)
            .AddUnitOfWork();
        
        return services;
    }
    
    private static IServiceCollection AddRepositories(
        this IServiceCollection services)
    {
        services.AddScoped<IDiscussionsRepository, DiscussionsRepository>();
        return services;
    }
    
    private static IServiceCollection AddDataBase(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddScoped<DiscussionsWriteDbContext>(_ =>
            new DiscussionsWriteDbContext(configuration.GetConnectionString(Constants.DATABASE)!));

        services.AddScoped<IDiscussionsReadDbContext, DiscussionsReadDbContext>(_ =>
            new DiscussionsReadDbContext(configuration.GetConnectionString(Constants.DATABASE)!));

        return services;
    }
    
    private static IServiceCollection AddUnitOfWork(
        this IServiceCollection services)
    {
        services.AddKeyedScoped<IUnitOfWork, UnitOfWork>(Modules.Discussions);
        return services;
    }
}