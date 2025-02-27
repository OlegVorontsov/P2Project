namespace FilesService.Builders;

public static class BuildersRegister
{
    public static IServiceCollection AddBuilders(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.AddEndpoints();
        services.AddCors();
        services.AddAmazonS3(configuration);
        services.SetMinioOptions(configuration);
        
        return services;
    }
}