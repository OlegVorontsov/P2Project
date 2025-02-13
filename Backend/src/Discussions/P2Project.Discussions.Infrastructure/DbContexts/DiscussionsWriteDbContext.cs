using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using P2Project.Discussions.Domain;

namespace P2Project.Discussions.Infrastructure.DbContexts;

public class DiscussionsWriteDbContext : DbContext
{
    private readonly string _connectionString;
    
    public DbSet<Discussion> Discussions => Set<Discussion>();
    
    public DiscussionsWriteDbContext(string connectionString)
    {
        _connectionString = connectionString;
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(_connectionString);
        optionsBuilder.UseSnakeCaseNamingConvention();
        optionsBuilder.EnableSensitiveDataLogging(false);
        optionsBuilder.UseLoggerFactory(CreateLoggerFactory());
    }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.HasDefaultSchema("discussions");

        builder.ApplyConfigurationsFromAssembly(
            typeof(DiscussionsWriteDbContext).Assembly,
            type => type.FullName?.Contains("Configurations.Write") ?? false);
    }
    
    private ILoggerFactory CreateLoggerFactory() =>
        LoggerFactory.Create(builder => { builder.AddConsole(); });
}