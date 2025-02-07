using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using P2Project.Accounts.Application.Interfaces;
using P2Project.Accounts.Domain;
using P2Project.Accounts.Domain.Accounts;
using P2Project.Accounts.Domain.RolePermission;
using P2Project.Accounts.Domain.RolePermission.Permissions;
using P2Project.Accounts.Domain.RolePermission.Roles;
using P2Project.Core.Dtos.Accounts;
using P2Project.SharedKernel;

namespace P2Project.Accounts.Infrastructure.DbContexts;

public class AccountsReadDbContext(IConfiguration configuration) :
    DbContext, IAccountsReadDbContext
{
    public IQueryable<UserDto> Users => Set<UserDto>();
    public IQueryable<AdminAccountDto> AdminAccounts => Set<AdminAccountDto>();
    public IQueryable<VolunteerAccountDto> VolunteerAccounts => Set<VolunteerAccountDto>();
    public IQueryable<ParticipantAccountDto> ParticipantAccounts => Set<ParticipantAccountDto>();

    private ILoggerFactory CreateLoggerFactory() =>
        LoggerFactory.Create(builder => { builder.AddConsole(); });
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(configuration.GetConnectionString(Constants.DATABASE));
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