using System.Reflection;
using NotificationService.Application.Interfaces;
using NotificationService.Application.SendersManagement;
using P2Project.Core.Interfaces.Commands;
using P2Project.Core.Interfaces.Queries;

namespace NotificationService.Application;

public static class ApplicationDependencyInjection
{
    private static readonly Assembly _assembly = Assembly.GetExecutingAssembly();
    public static IServiceCollection AddApplication(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddCommands()
                .AddQueries()
                .AddSenders();
        
        return services;
    }
    
    private static IServiceCollection AddCommands(this IServiceCollection services)
    {
        return services.Scan(scan => scan.FromAssemblies(_assembly)
            .AddClasses(c => c
                .AssignableToAny(
                    typeof(ICommandHandler<,>),
                    typeof(ICommandHandler<>),
                    typeof(ICommandVoidHandler<>),
                    typeof(ICommandResponseHandler<,>)))
            .AsSelfWithInterfaces()
            .WithScopedLifetime());
    }

    private static IServiceCollection AddQueries(this IServiceCollection services)
    {
        return services.Scan(scan => scan.FromAssemblies(_assembly)
            .AddClasses(c => c
                .AssignableToAny(
                    typeof(IQueryHandler<,>),
                    typeof(IQueryHandler<>)))
            .AsSelfWithInterfaces()
            .WithScopedLifetime());
    }
    
    private static IServiceCollection AddSenders(this IServiceCollection services)
    {
        services.Scan(scan => scan.FromAssemblies(_assembly)
            .AddClasses(c => c
                .AssignableToAny(
                    typeof(INotificationSender)))
            .AsSelfWithInterfaces()
            .WithScopedLifetime());
        
        services.AddScoped<SendersFactory>();
        return services;
    }
}