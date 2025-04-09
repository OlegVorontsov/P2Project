using MassTransit;
using NotificationService.Application.Interfaces;
using NotificationService.Core;
using NotificationService.Infrastructure.Consumers;
using NotificationService.Infrastructure.DbContexts;
using NotificationService.Infrastructure.Repositories;
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
                .AddMessageBus(configuration);
        
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
}