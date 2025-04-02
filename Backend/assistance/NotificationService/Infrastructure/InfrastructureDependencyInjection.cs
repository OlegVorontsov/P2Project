using NotificationService.Infrastructure.DbContexts;
using NotificationService.Infrastructure.Repositories;

namespace NotificationService.Infrastructure;

public static class InfrastructureDependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDataBase(configuration)
                .AddRepositories();
        
        return services;
    }
    
    private static IServiceCollection AddDataBase(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddScoped<NotificationWriteDbContext>(_ =>
            new NotificationWriteDbContext(configuration.GetConnectionString("Database")!));

        return services;
    }
    
    private static IServiceCollection AddRepositories(
        this IServiceCollection services)
    {
        services.AddScoped<NotificationRepository>();

        return services;
    }
}