using NotificationService.Core.Options;
using Serilog;
using Serilog.Events;

namespace NotificationService.Core;

public static class CoreDependencyInjection
{
    public static IServiceCollection AddCore(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        AddSerilogLogger(services, configuration);
        
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
    
    private static void AddSerilogLogger(this IServiceCollection services, IConfiguration config)
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .WriteTo.Debug()
            .Enrich.WithThreadName()
            .WriteTo.Seq(config.GetConnectionString("Seq")
                         ?? throw new ArgumentNullException("Seq connection string was not found"))
            .MinimumLevel.Override("Microsoft.AspNetCore.Hosting", LogEventLevel.Information)
            .MinimumLevel.Override("Microsoft.AspNetCore.Mvc", LogEventLevel.Information)
            .MinimumLevel.Override("Microsoft.AspNetCore.Routing", LogEventLevel.Information)
            .CreateLogger();

        services.AddSerilog();
    }
}