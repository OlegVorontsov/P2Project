using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using P2Project.Accounts.Application.Interfaces;
using P2Project.Core.Dtos.Accounts;

namespace P2Project.Accounts.Infrastructure.DbContexts;

public class AccountsReadDbContext :
    DbContext, IAccountsReadDbContext
{
    private readonly string _connectionString;

    public AccountsReadDbContext(string connectionString)
    {
        _connectionString = connectionString;
    }
    public IQueryable<UserDto> Users => Set<UserDto>();
    public IQueryable<AdminAccountDto> AdminAccounts => Set<AdminAccountDto>();
    public IQueryable<VolunteerAccountDto> VolunteerAccounts => Set<VolunteerAccountDto>();
    public IQueryable<ParticipantAccountDto> ParticipantAccounts => Set<ParticipantAccountDto>();

    private ILoggerFactory CreateLoggerFactory() =>
        LoggerFactory.Create(builder => { builder.AddConsole(); });
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(_connectionString);
        optionsBuilder.UseSnakeCaseNamingConvention();
        optionsBuilder.EnableSensitiveDataLogging(false);
        optionsBuilder.UseLoggerFactory(CreateLoggerFactory());
    }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        builder.HasDefaultSchema("accounts");
        
        builder.ApplyConfigurationsFromAssembly(
            typeof(AccountsReadDbContext).Assembly,
            x => x.FullName!.Contains("Configurations.Read"));
    }
}