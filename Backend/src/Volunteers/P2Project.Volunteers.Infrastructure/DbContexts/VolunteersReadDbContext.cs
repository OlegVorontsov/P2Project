using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using P2Project.Core.Dtos.Pets;
using P2Project.Core.Dtos.Volunteers;
using P2Project.Volunteers.Application;
using P2Project.Volunteers.Application.Interfaces;

namespace P2Project.Volunteers.Infrastructure.DbContexts
{
    public class VolunteersReadDbContext :
        DbContext, IVolunteersReadDbContext
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
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.HasDefaultSchema("volunteers");
            
            builder.ApplyConfigurationsFromAssembly(
                typeof(VolunteersReadDbContext).Assembly,
                type => type.FullName?.Contains("Configurations.Read") ?? false);
        }
        public IQueryable<VolunteerDto> Volunteers => Set<VolunteerDto>();
        public IQueryable<PetDto> Pets => Set<PetDto>();
    }
}
