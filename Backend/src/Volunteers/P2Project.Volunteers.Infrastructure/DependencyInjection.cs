using FilesService.Communication;
using FilesService.Core.Dtos;
using FilesService.Core.Interfaces;
using FilesService.Core.Options;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Minio;
using P2Project.Core;
using P2Project.Core.BackroundServices;
using P2Project.Core.Factories;
using P2Project.Core.Interfaces;
using P2Project.Core.Interfaces.DataBase;
using P2Project.Core.Interfaces.Services;
using P2Project.Core.MessageQueues;
using P2Project.Core.Options;
using P2Project.SharedKernel;
using P2Project.Volunteers.Application;
using P2Project.Volunteers.Application.EventHandlers.PetWasChanged;
using P2Project.Volunteers.Application.Interfaces;
using P2Project.Volunteers.Infrastructure.BackgroundServices;
using P2Project.Volunteers.Infrastructure.Consumers;
using P2Project.Volunteers.Infrastructure.DbContexts;

namespace P2Project.Volunteers.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddVolunteersInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddRepositories()
                .AddDataBase(configuration)
                .AddUnitOfWork()
                .AddHostedServices()
                .AddMinioVault(configuration)
                .AddMediatR()
                .AddMessageBus(configuration);

        services.AddScoped<IFilesCleanerService, FilesCleanerService>();
        services.AddScoped<DeleteExpiredSoftDeletedEntityService>();
        services.AddSingleton<IMessageQueue<IEnumerable<FileInfoDto>>,
            InMemoryMessageQueue<IEnumerable<FileInfoDto>>>();
        return services;
    }

    private static IServiceCollection AddHostedServices(
        this IServiceCollection services)
    {
        services.AddHostedService<FilesCleanerBackgroundService>();
        services.AddHostedService<DeleteExpiredSoftDeletedEntityBackgroundService>();
        return services;
    }

    private static IServiceCollection AddUnitOfWork(
        this IServiceCollection services)
    {
        services.AddKeyedScoped<IUnitOfWork, UnitOfWork>(Modules.Volunteers);
        return services;
    }

    private static IServiceCollection AddDataBase(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddScoped<VolunteersWriteDbContext>(_ =>
            new VolunteersWriteDbContext(configuration.GetConnectionString(Constants.DATABASE)!));

        services.AddScoped<IVolunteersReadDbContext, VolunteersReadDbContext>(_ =>
            new VolunteersReadDbContext(configuration.GetConnectionString(Constants.DATABASE)!));

        services.AddSingleton<ISqlConnectionFactory, SqlConnectionFactory>();
        
        Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;

        return services;
    }

    private static IServiceCollection AddRepositories(
        this IServiceCollection services)
    {
        services.AddScoped<IVolunteersRepository, VolunteersRepository>();
        return services;
    }

    private static IServiceCollection AddMinioVault(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<MinioOptions>(
            configuration.GetSection(MinioOptions.MINIO));

        services.AddMinio(options =>
        {
            var minioOptions = configuration
                                   .GetSection(MinioOptions.MINIO)
                                   .Get<MinioOptions>()
                               ?? throw new ApplicationException(
                                   "Minio configuration missed");

            options.WithEndpoint(minioOptions.EndPoint);
            options.WithCredentials(
                minioOptions.AccessKey,
                minioOptions.SecretKey);
            options.WithSSL(minioOptions.WithSSl);
        });

        services.AddScoped<IFileProvider, MinioProvider>();

        return services;
    }

    private static IServiceCollection AddMediatR(this IServiceCollection services)
    {
        services.AddMediatR(
            cfg =>
            {
                cfg.RegisterServicesFromAssembly(typeof(CacheInvalidation).Assembly);
            });

        return services;
    }

    private static IServiceCollection AddMessageBus(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddMassTransit<IVolunteersMessageBus>(configure =>
        {
            var options = configuration
                .GetSection(RabbitMqOptions.SECTION_NAME)
                .Get<RabbitMqOptions>()!;

            configure.SetKebabCaseEndpointNameFormatter();

            configure.AddConsumer<CreateVolunteerConsumer>();

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