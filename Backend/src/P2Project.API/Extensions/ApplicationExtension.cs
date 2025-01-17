using Microsoft.EntityFrameworkCore;
using P2Project.Volunteers.Infrastructure.DbContexts;

namespace P2Project.API.Extensions;

public static class ApplicationExtension
{
    public static async Task ApplyMigrations(this WebApplication application)
    {
        await using var scope = application.Services.CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<VolunteersWriteDbContext>();
        await dbContext.Database.MigrateAsync();
    }
}
