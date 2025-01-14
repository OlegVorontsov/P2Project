using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using P2Project.Core.Factories;
using P2Project.Core.Interfaces;
using P2Project.Core.Interfaces.DataBase;
using P2Project.SharedKernel;
using P2Project.Species.Application;
using P2Project.Species.Infrastructure.DbContexts;

namespace P2Project.Species.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddSpeciesInfrastructure(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.AddRepositories()
            .AddDataBase(configuration)
            .AddUnitOfWork();
        
        return services;
    }
    
    private static IServiceCollection AddUnitOfWork(
        this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }

    private static IServiceCollection AddDataBase(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddScoped<WriteDbContext>(_ =>
            new WriteDbContext(configuration.GetConnectionString(Constants.DATABASE)!));
            
        services.AddScoped<IReadDbContext, ReadDbContext>(_ =>
            new ReadDbContext(configuration.GetConnectionString(Constants.DATABASE)!));
            
        services.AddSingleton<ISqlConnectionFactory, SqlConnectionFactory>();
        Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;

        return services;
    }

    private static IServiceCollection AddRepositories(
        this IServiceCollection services)
    {
        services.AddScoped<ISpeciesRepository, SpeciesRepository>();

        return services;
    }
}