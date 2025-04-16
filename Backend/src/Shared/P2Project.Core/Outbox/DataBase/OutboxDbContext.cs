using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using P2Project.Core.Outbox.Models;

namespace P2Project.Core.Outbox.DataBase
{
    public class OutboxDbContext : DbContext
    {
        private readonly string _connectionString;
        
        public DbSet<OutboxMessage> OutboxMessages => Set<OutboxMessage>();

        public OutboxDbContext(string connectionString)
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
            builder.HasDefaultSchema("outbox");

            builder.ApplyConfigurationsFromAssembly(
                typeof(OutboxDbContext).Assembly,
                type => type.FullName?.Contains("Outbox.Configurations") ?? false);
        }
        private ILoggerFactory CreateLoggerFactory() =>
            LoggerFactory.Create(builder => { builder.AddConsole(); });
    }
}
