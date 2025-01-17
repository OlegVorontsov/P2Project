using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace P2Project.Species.Infrastructure.DbContexts
{
    public class SpeciesWriteDbContext : DbContext
    {
        private readonly string _connectionString;

        public SpeciesWriteDbContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        private ILoggerFactory CreateLoggerFactory() =>
            LoggerFactory.Create(builder => { builder.AddConsole(); });
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(_connectionString);
            optionsBuilder.UseSnakeCaseNamingConvention();
            optionsBuilder.EnableSensitiveDataLogging(false);
            optionsBuilder.UseLoggerFactory(CreateLoggerFactory());
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(
                typeof(SpeciesWriteDbContext).Assembly,
                type => type.FullName?.Contains("Configurations.Write") ?? false);
        }

        public DbSet<Domain.Species> Species => Set<Domain.Species>();
    }
}
