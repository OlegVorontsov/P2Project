using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using P2Project.Core.Interfaces.Commands;
using P2Project.Core.Interfaces.Queries;
using P2Project.Volunteers.Application.Files.DeleteFile;
using P2Project.Volunteers.Application.Files.GetFile;
using P2Project.Volunteers.Application.Files.UploadFile;

namespace P2Project.Volunteers.Application;

public static class DependencyInjection
{
    private static readonly Assembly _assembly = Assembly.GetExecutingAssembly();
    
    public static IServiceCollection AddVolunteersApplication(this IServiceCollection services)
    {
        services
            .AddCommands()
            .AddQueries()
            .AddValidatorsFromAssembly(_assembly);
        
        services.AddScoped<UploadFileHandler>();
        services.AddScoped<DeleteFileHandler>();
        services.AddScoped<GetFileHandler>();

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