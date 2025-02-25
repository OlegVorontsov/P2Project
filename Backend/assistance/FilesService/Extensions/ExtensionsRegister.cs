using FilesService.Builders;

namespace FilesService.Extensions;

public static class ExtensionsRegister
{
    public static WebApplication AddExtensions(this WebApplication app)
    {
        app.MapEndpoints();
        
        return app;
    }
}