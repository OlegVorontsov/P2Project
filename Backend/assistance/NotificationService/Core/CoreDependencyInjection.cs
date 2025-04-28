using System.Reflection;
using Elastic.CommonSchema.Serilog;
using Elastic.Ingest.Elasticsearch;
using Elastic.Ingest.Elasticsearch.DataStreams;
using Elastic.Serilog.Sinks;
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
        services.Configure<TelegramOptions>(configuration.GetSection(TelegramOptions.SECTION_NAME));

        return services;
    }
    
    private static void AddSerilogLogger(this IServiceCollection services, IConfiguration config)
    {
        var indexFormat = $"{Assembly.GetExecutingAssembly().GetName().Name?.ToLower()
            .Replace(".", "-")}-{DateTime.UtcNow:dd-mm-yyyy}";
        
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .WriteTo.Debug()
            .Enrich.WithThreadName()
            .WriteTo.Seq(config.GetConnectionString("Seq")
                         ?? throw new ArgumentNullException("Seq connection string was not found"))
            .WriteTo.Elasticsearch([new Uri("http://localhost:9200")], options =>
            {
                options.DataStream = new DataStreamName(indexFormat);
                options.TextFormatting = new EcsTextFormatterConfiguration();
                options.BootstrapMethod = BootstrapMethod.Silent;
            })
            .MinimumLevel.Override("Microsoft.AspNetCore.Hosting", LogEventLevel.Information)
            .MinimumLevel.Override("Microsoft.AspNetCore.Mvc", LogEventLevel.Information)
            .MinimumLevel.Override("Microsoft.AspNetCore.Routing", LogEventLevel.Information)
            .CreateLogger();

        services.AddSerilog();
    }
}