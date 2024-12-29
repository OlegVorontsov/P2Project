using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Minio;
using P2Project.Application.FileProvider;
using P2Project.Application.Interfaces;
using P2Project.Application.Interfaces.DataBase;
using P2Project.Application.Interfaces.DbContexts;
using P2Project.Application.Interfaces.DbContexts.Species;
using P2Project.Application.Interfaces.DbContexts.Volunteers;
using P2Project.Application.Interfaces.Services;
using P2Project.Application.Messaging;
using P2Project.Infrastructure.BackroundServices;
using P2Project.Infrastructure.DbContexts;
using P2Project.Infrastructure.MessageQueues;
using P2Project.Infrastructure.Options;
using P2Project.Infrastructure.Providers;
using P2Project.Infrastructure.Repositories;
using static P2Project.Infrastructure.BackroundServices.FilesCleanerBackgroundService;
using FileInfo = P2Project.Application.FileProvider.Models.FileInfo;

namespace P2Project.Infrastructure.Shared
{
    public static class Inject
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddRepositories()
                    .AddDataBase()
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
            this IServiceCollection services)
        {
            services.AddScoped<WriteDbContext>();
            services.AddScoped<IVolunteersReadDbContext, VolunteersReadDbContext>();
            services.AddScoped<ISpeciesReadDbContext, SpeciesReadDbContext>();
            
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
