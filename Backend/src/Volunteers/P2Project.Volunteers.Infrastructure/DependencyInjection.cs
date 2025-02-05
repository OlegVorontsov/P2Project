using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Minio;
using P2Project.Core;
using P2Project.Core.BackroundServices;
using P2Project.Core.Factories;
using P2Project.Core.Files;
using P2Project.Core.Files.Models;
using P2Project.Core.Interfaces;
using P2Project.Core.Interfaces.DataBase;
using P2Project.Core.Interfaces.Services;
using P2Project.Core.MessageQueues;
using P2Project.Core.Options;
using P2Project.SharedKernel;
using P2Project.Volunteers.Application;
using P2Project.Volunteers.Infrastructure.BackgroundServices;
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
            .AddMinioVault(configuration);

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
}