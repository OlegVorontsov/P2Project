using System.Reflection;
using Elastic.CommonSchema.Serilog;
using Elastic.Ingest.Elasticsearch;
using Elastic.Ingest.Elasticsearch.DataStreams;
using Elastic.Serilog.Sinks;
using Microsoft.OpenApi.Models;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using Serilog;
using Serilog.Events;

namespace P2Project.API;

public static class DependencyInjection
{
    public static IServiceCollection AddApi(this IServiceCollection services, IConfiguration config)
    {
        AddSerilogLogger(services, config);

        services.AddCustomSwaggerGen()
                .AddMetrics();

        return services;
    }
    
    public static void AddSerilogLogger(this IServiceCollection services, IConfiguration config)
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

    private static IServiceCollection AddCustomSwaggerGen(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "P2Project API",
                Version = "1"
            });

            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Insert JWT token value",
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] { }
                }
            });
        });

        return services;
    }

    private static IServiceCollection AddMetrics(this IServiceCollection services)
    {
        services.AddOpenTelemetry()
            .WithMetrics(b => b
                    .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("P2Project.API"))
                    .AddMeter("P2Project")
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddRuntimeInstrumentation()
                    .AddProcessInstrumentation()
                    .AddPrometheusExporter());

        return services;
    }
}