using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using P2Project.Accounts.Domain;
using P2Project.Accounts.Domain.Accounts;
using P2Project.Accounts.Domain.RolePermission;
using P2Project.Accounts.Domain.RolePermission.Permissions;
using P2Project.Accounts.Domain.RolePermission.Roles;
using P2Project.SharedKernel;

namespace P2Project.Accounts.Infrastructure.DbContexts;

public class AuthorizationDbContext(IConfiguration configuration) :
    IdentityDbContext<User, Role, Guid>
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<Permission> Permissions => Set<Permission>();
    public DbSet<RolePermission> RolePermissions => Set<RolePermission>();
    public DbSet<AdminAccount> AdminAccounts => Set<AdminAccount>();
    public DbSet<ParticipantAccount> ParticipantAccounts => Set<ParticipantAccount>();
    public DbSet<VolunteerAccount> VolunteerAccounts => Set<VolunteerAccount>();
    public DbSet<RefreshSession> RefreshSessions => Set<RefreshSession>();

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
            typeof(AuthorizationDbContext).Assembly,
            x => x.FullName!.Contains("Configurations.Write"));
        
        builder.Entity<Role>()
            .ToTable("roles");
        
        builder.Entity<IdentityUserClaim<Guid>>()
            .ToTable("user_claims");
        
        builder.Entity<IdentityUserToken<Guid>>()
            .ToTable("user_tokens");
        
        builder.Entity<IdentityUserLogin<Guid>>()
            .ToTable("user_logins");
        
        builder.Entity<IdentityRoleClaim<Guid>>()
            .ToTable("role_claims");
        
        builder.Entity<IdentityUserRole<Guid>>()
            .ToTable("user_roles");
    }
}