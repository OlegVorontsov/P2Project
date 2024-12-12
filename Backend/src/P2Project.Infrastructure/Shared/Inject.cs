using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Minio;
using P2Project.Application.FileProvider;
using P2Project.Application.FilesCleaner;
using P2Project.Application.Messaging;
using P2Project.Application.Shared;
using P2Project.Application.Species;
using P2Project.Application.Volunteers;
using P2Project.Domain.PetManagment.ValueObjects;
using P2Project.Infrastructure.BackroundServices;
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
            services.AddScoped<WriteDBContext>();
            services.AddScoped<IVolunteersRepository, VolunteersRepository>();
            services.AddScoped<ISpeciesRepository, SpeciesRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            //services.AddSingleton<SoftDeleteInterceptor>();
            services.AddMinio(configuration);

            services.AddHostedService<FilesCleanerBackgroundService>();
            services.AddSingleton<IMessageQueue<IEnumerable<FileInfo>>,
                                  InMemoryMessageQueue<IEnumerable<FileInfo>>>();
            services.AddScoped<IFilesCleanerService, FilesCleanerService>();
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
