using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using P2Project.Accounts.Domain.Role;
using P2Project.Accounts.Domain.User;
using P2Project.SharedKernel;

namespace P2Project.Accounts.Infrastructure.DbContexts;
        
public class AuthorizationDbContext(IConfiguration configuration) :
    IdentityDbContext<IdentityUser<Guid>, IdentityRole<Guid>, Guid>
{       
    public DbSet<User> Users => Set<User>();
    public DbSet<Role> Roles => Set<Role>();

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
        
        builder.Entity<IdentityUser<Guid>>()
            .ToTable("users");
        
        builder.Entity<IdentityUserClaim<Guid>>()
            .ToTable("user_claims");
        
        builder.Entity<IdentityUserToken<Guid>>()
            .ToTable("user_tokens");
        
        builder.Entity<IdentityUserLogin<Guid>>()
            .ToTable("user_logins");
        
        builder.Entity<IdentityUserRole<Guid>>()
            .ToTable("user_roles");
        
        builder.Entity<IdentityRole<Guid>>()
            .ToTable("roles");
        
        builder.Entity<IdentityRoleClaim<Guid>>()
            .ToTable("role_claims");
    }
}