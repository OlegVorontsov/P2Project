using FilesService.Communication;

namespace P2Project.API;

public static class HttpCommunicationsDependencyInjection
{
    public static IServiceCollection AddHttpCommunications(
        this IServiceCollection services, IConfiguration config)
    {
        services.AddAmazonHttpCommunication(config);

        return services;
    }
}