using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using P2Project.Core.Interfaces.Outbox;
using P2Project.Core.Options;
using P2Project.Core.Outbox.DataBase;
using P2Project.Core.Outbox.ProcessMessages;
using P2Project.SharedKernel;
using Quartz;

namespace P2Project.Core;

public static class CoreDependencyInjection
{
    public static IServiceCollection AddCore(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddRepositories()
                .AddDataBase(configuration)
                .AddOutbox()
                .AddQuartzService()
                .AddMessageBus(configuration)
                .AddDistributedCache(configuration);

        return services;
    }

    private static IServiceCollection AddRepositories(
        this IServiceCollection services)
    {
        services.AddScoped<IOutboxRepository, OutboxRepository>();
        return services;
    }

    private static IServiceCollection AddDataBase(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddScoped<OutboxDbContext>(_ =>
            new OutboxDbContext(configuration.GetConnectionString(Constants.DATABASE)!));

        return services;
    }

    private static IServiceCollection AddMessageBus(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMassTransit<IOutboxMessageBus>(configure =>
        {
            var options = configuration
                .GetSection(RabbitMqOptions.SECTION_NAME)
                .Get<RabbitMqOptions>()!;

            configure.SetKebabCaseEndpointNameFormatter();

            configure.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(new Uri(options.Host), h =>
                {
                    h.Username(options.Username);
                    h.Password(options.Password);
                });

                cfg.ConfigureEndpoints(context);
            });
        });

        return services;
    }

    private static IServiceCollection AddOutbox(
        this IServiceCollection services)
    {
        services.AddScoped<ProcessOutboxMessagesService>();
        return services;
    }

    private static IServiceCollection AddQuartzService(this IServiceCollection services)
    {
        services.AddScoped<ProcessOutboxMessagesService>();

        services.AddQuartz(configure =>
        {
            var jobKey = new JobKey(nameof(ProcessOutboxMessagesJob));

            configure
                .AddJob<ProcessOutboxMessagesJob>(jobKey)
                .AddTrigger(trigger => trigger.ForJob(jobKey).WithSimpleSchedule(
                    schedule => schedule.WithIntervalInSeconds(1).RepeatForever()));
        });

        services.AddQuartzHostedService(options => { options.WaitForJobsToComplete = true; });

        return services;
    }

    private static IServiceCollection AddDistributedCache(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.AddStackExchangeRedisCache(options =>
            {
                string connection = configuration.GetConnectionString("Redis") ??
                                    throw new ArgumentNullException(nameof(connection));

                options.Configuration = connection;
            });

        return services;
    }
}