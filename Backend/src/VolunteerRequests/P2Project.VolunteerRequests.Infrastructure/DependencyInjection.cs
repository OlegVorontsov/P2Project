using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using P2Project.Core;
using P2Project.Core.Interfaces;
using P2Project.Core.Options;
using P2Project.SharedKernel;
using P2Project.VolunteerRequests.Application;
using P2Project.VolunteerRequests.Application.Interfaces;
using P2Project.VolunteerRequests.Infrastructure.DbContexts;
using P2Project.VolunteerRequests.Infrastructure.Outbox;
using Quartz;

namespace P2Project.VolunteerRequests.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddVolunteerRequestsInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddRepositories()
                .AddDataBase(configuration)
                .AddUnitOfWork()
                .AddOutbox()
                .AddQuartzService()
                .AddMessageBus(configuration);
        
        return services;
    }
    
    private static IServiceCollection AddRepositories(
        this IServiceCollection services)
    {
        services.AddScoped<IVolunteerRequestsRepository, VolunteerRequestsRepository>();
        services.AddScoped<IOutboxRepository, OutboxRepository>();
        return services;
    }
    
    private static IServiceCollection AddDataBase(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddScoped<VolunteerRequestsWriteDbContext>(_ =>
            new VolunteerRequestsWriteDbContext(configuration.GetConnectionString(Constants.DATABASE)!));

        services.AddScoped<IVolunteerRequestsReadDbContext, VolunteerRequestsReadDbContext>(_ =>
            new VolunteerRequestsReadDbContext(configuration.GetConnectionString(Constants.DATABASE)!));

        return services;
    }
    
    private static IServiceCollection AddUnitOfWork(
        this IServiceCollection services)
    {
        services.AddKeyedScoped<IUnitOfWork, UnitOfWork>(Modules.VolunteerRequests);
        return services;
    }
    
    private static IServiceCollection AddMessageBus(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMassTransit<IVolunteerRequestMessageBus>(configure =>
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
}