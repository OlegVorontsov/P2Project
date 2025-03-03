using FilesService.Builders;

namespace FilesService.Extensions;

public static class ExtensionsRegister
{
    public static WebApplication AddExtensions(this WebApplication app)
    {
        app.MapEndpoints();
        
        app.UseCors(config =>
        {
            config.AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });
        
        return app;
    }
}