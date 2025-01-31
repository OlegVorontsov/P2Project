using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using P2Project.Core.Interfaces.Commands;

namespace P2Project.Accounts.Application;

public static class DependencyInjection
{
    private static readonly Assembly _assembly = Assembly.GetExecutingAssembly();
    
    public static IServiceCollection AddAccountsApplication(
        this IServiceCollection services)
    {
        services.AddCommands();
            //.AddQueries()
            //.AddValidatorsFromAssembly(_assembly);

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
}