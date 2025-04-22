using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using P2Project.VolunteerRequests.Domain;

namespace P2Project.VolunteerRequests.Infrastructure.DbContexts
{
    public class VolunteerRequestsWriteDbContext : DbContext
    {
        private readonly string _connectionString;
        
        public DbSet<VolunteerRequest> VolunteerRequests => Set<VolunteerRequest>();

        public VolunteerRequestsWriteDbContext(string connectionString)
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
            builder.HasDefaultSchema("volunteer_requests");

            builder.ApplyConfigurationsFromAssembly(
                typeof(VolunteerRequestsWriteDbContext).Assembly,
                type => type.FullName?.Contains("Configurations.Write") ?? false);
        }
        private ILoggerFactory CreateLoggerFactory() =>
            LoggerFactory.Create(builder => { builder.AddConsole(); });
    }
}
