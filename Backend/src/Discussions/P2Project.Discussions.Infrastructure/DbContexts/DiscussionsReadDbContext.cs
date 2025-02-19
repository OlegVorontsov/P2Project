using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using P2Project.Core.Dtos.Discussions;
using P2Project.Discussions.Application;

namespace P2Project.Discussions.Infrastructure.DbContexts;

public class DiscussionsReadDbContext :
    DbContext, IDiscussionsReadDbContext
{
    private readonly string _connectionString;

    public DiscussionsReadDbContext(string connectionString)
    {
        _connectionString = connectionString;
    }
    
    public IQueryable<DiscussionDto> Discussions => Set<DiscussionDto>();
    public IQueryable<MessageDto> Messages => Set<MessageDto>();
    
    private ILoggerFactory CreateLoggerFactory() =>
        LoggerFactory.Create(builder => { builder.AddConsole(); });
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(_connectionString);
        optionsBuilder.UseSnakeCaseNamingConvention();
        optionsBuilder.EnableSensitiveDataLogging(false);
        optionsBuilder.UseLoggerFactory(CreateLoggerFactory());

        optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
    }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.HasDefaultSchema("discussions");
            
        builder.ApplyConfigurationsFromAssembly(
            typeof(DiscussionsReadDbContext).Assembly,
            type => type.FullName?.Contains("Configurations.Read") ?? false);
    }
}