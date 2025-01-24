using System.Text.Json;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using P2Project.Accounts.Domain.RolePermission;
using P2Project.Accounts.Domain.RolePermission.Permissions;
using P2Project.Accounts.Domain.RolePermission.Roles;
using P2Project.Accounts.Domain.User;
using P2Project.Accounts.Domain.User.ValueObjects;
using P2Project.SharedKernel;

namespace P2Project.Accounts.Infrastructure.DbContexts;

public class AuthorizationDbContext(IConfiguration configuration) :
    IdentityDbContext<User, Role, Guid>
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
        
        builder.HasDefaultSchema("accounts");
        
        builder.Entity<User>()
            .ToTable("users");

        builder.Entity<User>()
            .Property(u => u.SocialNetworks)
            .HasConversion(
                u => JsonSerializer
                    .Serialize(u, JsonSerializerOptions.Default),
                json => JsonSerializer
                    .Deserialize<IReadOnlyList<SocialNetwork>>(json, JsonSerializerOptions.Default)!);
        
        builder.Entity<IdentityUserClaim<Guid>>()
            .ToTable("user_claims");
        
        builder.Entity<IdentityUserToken<Guid>>()
            .ToTable("user_tokens");
        
        builder.Entity<IdentityUserLogin<Guid>>()
            .ToTable("user_logins");
        
        builder.Entity<IdentityUserRole<Guid>>()
            .ToTable("user_roles");
        
        builder.Entity<Role>()
            .ToTable("roles");
        
        builder.Entity<IdentityRoleClaim<Guid>>()
            .ToTable("role_claims");
        
        builder.Entity<Permission>()
            .ToTable("permissions");
        
        builder.Entity<Permission>()
            .HasIndex(p => p.Code)
            .IsUnique();
        
        builder.Entity<Permission>()
            .Property(p => p.Description)
            .HasMaxLength(500);
        
        builder.Entity<RolePermission>()
            .ToTable("role_permissions");
        
        builder.Entity<RolePermission>()
            .HasOne(rp => rp.Role)
            .WithMany(r => r.RolePermissions)
            .HasForeignKey(rp => rp.RoleId);
        
        builder.Entity<RolePermission>()
            .HasOne(rp => rp.Permission)
            .WithMany()
            .HasForeignKey(rp => rp.PermissionId);
        
        builder.Entity<RolePermission>()
            .HasKey(rp => new { rp.RoleId, rp.PermissionId });
    }
}