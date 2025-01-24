using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using P2Project.Core.Dtos.Pets;
using P2Project.Species.Application;

namespace P2Project.Species.Infrastructure.DbContexts
{
    public class SpeciesReadDbContext :
        DbContext, ISpeciesReadDbContext
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
    
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(
                typeof(SpeciesReadDbContext).Assembly,
                type => type.FullName?.Contains("Configurations.Read") ?? false);
        }

        public IQueryable<SpeciesDto> Species => Set<SpeciesDto>();
        public IQueryable<BreedReadDto> Breeds => Set<BreedReadDto>();
    }
}