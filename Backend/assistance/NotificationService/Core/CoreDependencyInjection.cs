using NotificationService.Core.Options;

namespace NotificationService.Core;

public static class CoreDependencyInjection
{
    public static IServiceCollection AddCore(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddOptions(configuration);
        
        return services;
    }
    
    private static IServiceCollection AddOptions(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<EmailOptions>(configuration.GetSection(EmailOptions.YANDEX));

        return services;
    }
}