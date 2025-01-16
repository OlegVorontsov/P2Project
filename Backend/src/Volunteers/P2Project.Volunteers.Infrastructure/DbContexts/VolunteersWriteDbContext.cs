using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using P2Project.Species.Infrastructure.DbContexts;
using P2Project.Volunteers.Domain;
using _Species = P2Project.Species.Domain.Species;

namespace P2Project.Volunteers.Infrastructure.DbContexts
{
    public class VolunteersWriteDbContext : DbContext
    {
        private readonly string _connectionString;

        public VolunteersWriteDbContext(string connectionString)
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
                typeof(VolunteersWriteDbContext).Assembly,
                type => type.FullName?.Contains("Configurations.Write") ?? false);
            modelBuilder.ApplyConfigurationsFromAssembly(
                typeof(SpeciesWriteDbContext).Assembly,
                type => type.FullName?.Contains("Configurations.Write") ?? false);
        }
        public DbSet<Volunteer> Volunteers => Set<Volunteer>();
        public DbSet<_Species> Species => Set<_Species>();
    }
}
