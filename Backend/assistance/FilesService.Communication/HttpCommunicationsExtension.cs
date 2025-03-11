using FilesService.Communication.HttpClients;
using FilesService.Core.Interfaces;
using FilesService.Core.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace FilesService.Communication;

public static class HttpCommunicationsExtension
{
    public static IServiceCollection AddAmazonHttpCommunication(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<FilesServiceOptions>(configuration.GetSection(FilesServiceOptions.SECTION_NAME));
        
        services.AddHttpClient<IFilesHttpClient, FilesHttpClient>((sp, config) =>
        {
            var filesServiceOptions = sp.GetRequiredService<IOptions<FilesServiceOptions>>().Value;
            config.BaseAddress = new Uri(filesServiceOptions.Url);
        });

        services.AddSingleton<IFilesHttpClient, FilesHttpClient>();

        return services;
    }
}