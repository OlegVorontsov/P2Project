using Hangfire;
using Hangfire.PostgreSql;

namespace FilesService.Builders;

public static class HangfireBuilder
{
    public static IServiceCollection AddHangfire(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHangfire(config => config
            .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .UsePostgreSqlStorage(c =>
                c.UseNpgsqlConnection(configuration.GetConnectionString("hangfire"))));

        //Add the processing server as IHostedService
        services.AddHangfireServer();

        return services;
    }
}