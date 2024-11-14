using Microsoft.EntityFrameworkCore;
using P2Project.Infrastructure;

public static class AppExtensions
{
    public static async Task ApplyMigrations(
        this WebApplication app)
    {
        await using var scope = app.Services.CreateAsyncScope();
        var dbCondext = scope
                       .ServiceProvider
                       .GetRequiredService<ApplicationDBContext>();
        await dbCondext.Database.MigrateAsync();
    }
}