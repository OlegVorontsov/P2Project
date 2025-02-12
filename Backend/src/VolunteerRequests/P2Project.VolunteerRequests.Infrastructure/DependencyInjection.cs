using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using P2Project.Core;
using P2Project.Core.Interfaces;
using P2Project.SharedKernel;
using P2Project.VolunteerRequests.Application.Interfaces;
using P2Project.VolunteerRequests.Infrastructure.DbContexts;

namespace P2Project.VolunteerRequests.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddVolunteerRequestsInfrastructure(
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
        services.AddScoped<IVolunteerRequestsRepository, VolunteerRequestsRepository>();
        return services;
    }
    
    private static IServiceCollection AddDataBase(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddScoped<VolunteerRequestsWriteDbContext>(_ =>
            new VolunteerRequestsWriteDbContext(configuration.GetConnectionString(Constants.DATABASE)!));

        /*services.AddScoped<IVolunteersReadDbContext, VolunteersReadDbContext>(_ =>
            new VolunteersReadDbContext(configuration.GetConnectionString(Constants.DATABASE)!));*/

        return services;
    }
    
    private static IServiceCollection AddUnitOfWork(
        this IServiceCollection services)
    {
        services.AddKeyedScoped<IUnitOfWork, UnitOfWork>(Modules.VolunteerRequests);
        return services;
    }
}