using FilesService.Application.Interfaces;
using FilesService.Infrastructure.MongoDb;
using MongoDB.Driver;

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
        
        services.AddSingleton<IMongoClient>(new MongoClient(
            configuration.GetConnectionString("Mongo")));
        services.AddScoped<MongoDbContext>();
        services.AddScoped<IFilesRepository, MongoDbRepository>();

        services.AddHangfire(configuration);
        
        return services;
    }
}