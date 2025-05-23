using MassTransit;
using NotificationService.Application.Interfaces;
using NotificationService.Core;
using NotificationService.Infrastructure.Consumers;
using NotificationService.Infrastructure.DbContexts;
using NotificationService.Infrastructure.Repositories;
using NotificationService.Infrastructure.TelegramNotification;
using P2Project.Core.Options;

namespace NotificationService.Infrastructure;

public static class InfrastructureDependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDataBase(configuration)
                .AddRepositories()
                .AddScoped<UnitOfWork>()
                .AddMessageBus(configuration)
                .AddTelegramManager();
        
        return services;
    }
    
    private static IServiceCollection AddDataBase(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddScoped<NotificationWriteDbContext>(_ =>
            new NotificationWriteDbContext(configuration.GetConnectionString(Constants.DATABASE)!));

        return services;
    }
    
    private static IServiceCollection AddRepositories(
        this IServiceCollection services)
    {
        services.AddScoped<NotificationRepository>();

        return services;
    }
    
    private static IServiceCollection AddMessageBus(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddMassTransit<INotificationMessageBus>(configure =>
        {
            var options = configuration
                .GetSection(RabbitMqOptions.SECTION_NAME)
                .Get<RabbitMqOptions>()!;
            
            configure.SetKebabCaseEndpointNameFormatter();
            
            configure.AddConsumer<ConfirmUserEmailConsumer>();
            configure.AddConsumer<CreatedUserConsumer>();
            
            configure.AddConsumer<CreateVolunteerAccountNotificationConsumer>();
            configure.AddConsumer<OpenDiscussionNotificationConsumer>();
            configure.AddConsumer<AddDiscussionMessageNotificationConsumer>();
            
            configure.AddConfigureEndpointsCallback((context, name, cfg) =>
            {
                cfg.UseDelayedRedelivery(r => r.Intervals(
                    TimeSpan.FromSeconds(5),
                    TimeSpan.FromSeconds(10),
                    TimeSpan.FromSeconds(15)));
            });

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

    private static IServiceCollection AddTelegramManager(
        this IServiceCollection services)
    {
        return services.AddScoped<TelegramManager>();
    }
}