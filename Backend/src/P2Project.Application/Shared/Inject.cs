using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using P2Project.Application.Agreements;
using P2Project.Application.Files.DeleteFile;
using P2Project.Application.Files.GetFile;
using P2Project.Application.Files.UploadFile;
using P2Project.Application.Interfaces.Agreements;
using P2Project.Application.Interfaces.Commands;
using P2Project.Application.Interfaces.Queries;

namespace P2Project.Application.Shared
{
    public static class Inject
    {
        public static IServiceCollection AddApplication(
            this IServiceCollection services)
        {
            services.AddCommands()
                    .AddQueries()
                    .AddValidatorsFromAssembly(typeof(Inject).Assembly);

            services.AddScoped<IPetsAgreement, PetsAgreement>();
            services.AddScoped<ISpeciesAgreement, SpeciesAgreement>();
            
            services.AddScoped<UploadFileHandler>();
            services.AddScoped<DeleteFileHandler>();
            services.AddScoped<GetFileHandler>();
            
            return services;
        }

        private static IServiceCollection AddCommands(this IServiceCollection services)
        {
            return services.Scan(scan => scan.FromAssemblies(typeof(Inject).Assembly)
                .AddClasses(classes => classes
                    .AssignableToAny([typeof(ICommandHandler<>), typeof(ICommandHandler<,>)]))
                .AsSelfWithInterfaces()
                .WithScopedLifetime());
        }
        
        private static IServiceCollection AddQueries(this IServiceCollection services)
        {
            services.Scan(scan => scan.FromAssemblies(typeof(Inject).Assembly)
                .AddClasses(classes => classes
                    .AssignableTo(typeof(IQueryHandler<,>)))
                .AsSelfWithInterfaces()
                .WithScopedLifetime());
            
            return services.Scan(scan => scan.FromAssemblies(typeof(Inject).Assembly)
                .AddClasses(classes => classes
                    .AssignableTo(typeof(IQueryValidationHandler<,>)))
                .AsSelfWithInterfaces()
                .WithScopedLifetime());
        }
    }
}
