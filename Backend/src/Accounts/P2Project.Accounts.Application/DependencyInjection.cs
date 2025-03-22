using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using P2Project.Core.Interfaces.Commands;
using P2Project.Core.Interfaces.Queries;

namespace P2Project.Accounts.Application;

public static class DependencyInjection
{
    private static readonly Assembly _assembly = Assembly.GetExecutingAssembly();
    
    public static IServiceCollection AddAccountsApplication(
        this IServiceCollection services)
    {
        services.AddCommands()
                .AddQueries()
                .AddValidatorsFromAssembly(_assembly);
        
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));

        return services;
    }
    
    private static IServiceCollection AddCommands(this IServiceCollection services)
    {
        return services.Scan(scan => scan.FromAssemblies(_assembly)
            .AddClasses(c => c
                .AssignableToAny(typeof(ICommandHandler<,>), typeof(ICommandHandler<>)))
            .AsSelfWithInterfaces()
            .WithScopedLifetime());
    }
    
    private static IServiceCollection AddQueries(this IServiceCollection services)
    {
        return services.Scan(scan => scan.FromAssemblies(_assembly)
            .AddClasses(c => c
                .AssignableToAny(typeof(IQueryHandler<,>), typeof(IQueryValidationHandler<,>)))
            .AsSelfWithInterfaces()
            .WithScopedLifetime());
    }
}