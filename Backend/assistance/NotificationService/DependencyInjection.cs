using NotificationService.Infrastructure.DbContexts;

namespace NotificationService;

public static class DependencyInjection
{
    public static IServiceCollection AddServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDataBase(configuration);
        
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
}