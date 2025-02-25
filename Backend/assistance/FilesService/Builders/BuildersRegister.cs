using FilesService.Builders;

namespace FilesService.Builders;

public static class BuildersRegister
{
    public static IServiceCollection AddBuilders(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.AddEndpoints();
        services.AddAmazonS3(configuration);
        
        return services;
    }
}