using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using P2Project.Accounts.Domain.RolePermission.Permissions;

namespace P2Project.Accounts.Infrastructure.Configurations.Write;

public class PermissionConfiguration :
    IEntityTypeConfiguration<Permission>
{
    public void Configure(EntityTypeBuilder<Permission> builder)
    {
        builder.ToTable("permissions");
        
        builder.HasIndex(p => p.Code)
               .IsUnique();
        
        builder.Property(p => p.Description)
               .HasMaxLength(500);
    }
}