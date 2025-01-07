using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using P2Project.Application.Interfaces.DbContexts.Volunteers;
using P2Project.Application.Shared.Dtos.Pets;
using P2Project.Application.Shared.Dtos.Volunteers;

namespace P2Project.Infrastructure.DbContexts
{
    public class VolunteersReadDbContext : DbContext, IVolunteersReadDbContext
    {
        private readonly string _connectionString;

        public VolunteersReadDbContext(string connectionString)
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
                typeof(VolunteersReadDbContext).Assembly,
                type => type.FullName?.Contains("Configurations.Read") ?? false);
        }
        public IQueryable<VolunteerDto> Volunteers => Set<VolunteerDto>();
        public IQueryable<PetDto> Pets => Set<PetDto>();
    }
}
