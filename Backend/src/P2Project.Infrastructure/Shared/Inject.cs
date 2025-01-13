using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Minio;
using P2Project.Core.BackroundServices;
using P2Project.Core.Factories;
using P2Project.Core.Files;
using P2Project.Core.Interfaces;
using P2Project.Core.Interfaces.DataBase;
using P2Project.Core.Interfaces.Services;
using P2Project.Core.MessageQueues;
using P2Project.Core.Messaging;
using P2Project.Core.Options;
using P2Project.Infrastructure.DbContexts;
using P2Project.Infrastructure.Providers;
using P2Project.Infrastructure.Repositories;
using P2Project.Shared.Interfaces;
using P2Project.Shared.Interfaces.DataBase;
using P2Project.Shared.Interfaces.DbContexts.Species;
using P2Project.Shared.Interfaces.DbContexts.Volunteers;
using P2Project.Shared.Interfaces.Services;
using static P2Project.Core.BackroundServices.FilesCleanerBackgroundService;
using FileInfo = P2Project.Core.Files.Models.FileInfo;

namespace P2Project.Infrastructure.Shared
{
    public static class Inject
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddRepositories()
                    .AddDataBase(configuration)
                    .AddUnitOfWork()
                    .AddHostedServices()
                    .AddMinio(configuration);

            services.AddSingleton<IMessageQueue<IEnumerable<FileInfo>>,
                                  InMemoryMessageQueue<IEnumerable<FileInfo>>>();
            services.AddScoped<IFilesCleanerService, FilesCleanerService>();
            return services;
        }

        private static IServiceCollection AddHostedServices(
            this IServiceCollection services)
        {
            services.AddHostedService<FilesCleanerBackgroundService>();

            return services;
        }

        private static IServiceCollection AddUnitOfWork(
            this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }

        private static IServiceCollection AddDataBase(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddScoped<WriteDbContext>(_ =>
                new WriteDbContext(configuration.GetConnectionString(Constants.DATABASE)!));
            
            services.AddScoped<IVolunteersReadDbContext, VolunteersReadDbContext>(_ =>
                new VolunteersReadDbContext(configuration.GetConnectionString(Constants.DATABASE)!));
            
            services.AddScoped<ISpeciesReadDbContext, SpeciesReadDbContext>(_ =>
                new SpeciesReadDbContext(configuration.GetConnectionString(Constants.DATABASE)!));
            
            services.AddSingleton<ISqlConnectionFactory, SqlConnectionFactory>();
            Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;

            return services;
        }

        private static IServiceCollection AddRepositories(
            this IServiceCollection services)
        {
            services.AddScoped<IVolunteersRepository, VolunteersRepository>();
            services.AddScoped<ISpeciesRepository, SpeciesRepository>();

            return services;
        }

        private static IServiceCollection AddMinio(
            this IServiceCollection services,
            IConfiguration configuration)
        {
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
}
