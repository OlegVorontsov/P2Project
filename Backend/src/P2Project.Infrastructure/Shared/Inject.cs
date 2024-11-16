using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Minio;
using P2Project.Application.Volunteers;
using P2Project.Infrastructure.Options;
using P2Project.Infrastructure.Repositories;

namespace P2Project.Infrastructure.Shared
{
    public static class Inject
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddScoped<ApplicationDBContext>();
            services.AddScoped<IVolunteersRepository, VolunteersRepository>();
            //services.AddSingleton<SoftDeleteInterceptor>();
            services.AddMinio(configuration);
            return services;
        }
        private static IServiceCollection AddMinio(
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
                ?? throw new ApplicationException("Minio configuration missed");

                options.WithEndpoint(minioOptions.EndPoint);
                options.WithCredentials(
                    minioOptions.AccessKey,
                    minioOptions.SecretKey);
                options.WithSSL(minioOptions.WithSSl);
            });
            return services;
        }
    }
}
