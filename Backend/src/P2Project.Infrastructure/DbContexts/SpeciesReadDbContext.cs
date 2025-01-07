using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using P2Project.Application.Interfaces.DbContexts.Species;
using P2Project.Application.Shared.Dtos.Pets;
using P2Project.Infrastructure.Shared;

namespace P2Project.Infrastructure.DbContexts
{
    public class SpeciesReadDbContext : DbContext, ISpeciesReadDbContext
    {
        private readonly string _connectionString;

        public SpeciesReadDbContext(string connectionString)
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

            optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        }
    
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(
                typeof(SpeciesReadDbContext).Assembly,
                type => type.FullName?.Contains("Configurations.Read") ?? false);
        }

        public IQueryable<SpeciesDto> Species => Set<SpeciesDto>();
        public IQueryable<BreedReadDto> Breeds => Set<BreedReadDto>();
    }
}