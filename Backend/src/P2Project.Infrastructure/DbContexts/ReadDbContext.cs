using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using P2Project.Application.Interfaces.DbContexts;
using P2Project.Application.Shared.Dtos;
using P2Project.Infrastructure.Shared;

namespace P2Project.Infrastructure.DbContexts
{
    public class ReadDbContext(
    IConfiguration configuration) : DbContext, IReadDbContext
    {
        private ILoggerFactory CreateLoggerFactory() =>
            LoggerFactory.Create(builder => { builder.AddConsole(); });
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(
                configuration.GetConnectionString(Constants.DATABASE));
            optionsBuilder.UseSnakeCaseNamingConvention();
            optionsBuilder.EnableSensitiveDataLogging(false);
            optionsBuilder.UseLoggerFactory(CreateLoggerFactory());

            optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(
                typeof(ReadDbContext).Assembly,
                type => type.FullName?.Contains("Configurations.Read") ?? false);
        }
        public IQueryable<VolunteerDto> Volunteers => Set<VolunteerDto>();
        public IQueryable<PetDto> Pets => Set<PetDto>();
    }
}
