using Microsoft.EntityFrameworkCore;
using P2Project.Infrastructure.DBContexts;

namespace P2Project.API.Extensions;

public static class AppExtensions
{
    public static async Task ApplyMigrations(
        this WebApplication app)
    {
        await using var scope = app.Services.CreateAsyncScope();
        var dbCondext = scope
            .ServiceProvider
            .GetRequiredService<WriteDbContext>();
        await dbCondext.Database.MigrateAsync();
    }
}