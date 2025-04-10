using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NotificationService.Application.Interfaces;
using NotificationService.Infrastructure.Consumers;
using P2Project.Accounts.Application.Interfaces;
using P2Project.Accounts.Infrastructure.Admin;
using P2Project.Accounts.Infrastructure.Consumers;
using P2Project.Accounts.Infrastructure.DbContexts;
using P2Project.Core;
using P2Project.Core.Interfaces;
using P2Project.Core.Options;
using P2Project.SharedKernel;

namespace P2Project.Accounts.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddAccountsInfrastructure(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtOptions>(
            configuration.GetSection(JwtOptions.NAME));
        services.Configure<AdminOptions>(
            configuration.GetSection(AdminOptions.NAME));
        
        services.AddDataBase(configuration)
                .AddUnitOfWork()
                .AddMessageBus(configuration);
        
        return services;
    }
    
    private static IServiceCollection AddDataBase(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddScoped<AccountsWriteDbContext>();
        services.AddScoped<IAccountsReadDbContext, AccountsReadDbContext>(_ =>
            new AccountsReadDbContext(configuration.GetConnectionString(Constants.DATABASE)!));

        return services;
    }
    
    private static IServiceCollection AddUnitOfWork(
        this IServiceCollection services)
    {
        services.AddKeyedScoped<IUnitOfWork, UnitOfWork>(Modules.Accounts);

        return services;
    }
    
    private static IServiceCollection AddMessageBus(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddMassTransit<IAccountsMessageBus>(configure =>
        {
            var options = configuration
                .GetSection(RabbitMqOptions.SECTION_NAME)
                .Get<RabbitMqOptions>()!;
            
            configure.SetKebabCaseEndpointNameFormatter();

            configure.AddConsumer<CreateVolunteerAccountConsumer>();
            
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
}