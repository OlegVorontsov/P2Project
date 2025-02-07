using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using P2Project.Core.Dtos.Accounts;

namespace P2Project.Accounts.Infrastructure.Configurations.Read;

public class AdminAccountDtoConfiguration :
    IEntityTypeConfiguration<AdminAccountDto>
{
    public void Configure(EntityTypeBuilder<AdminAccountDto> builder)
    {
        builder.ToTable("admin_accounts");
        
        builder.HasKey(v => v.AdminAccountId);
        
        builder.Property(x => x.AdminAccountId)
            .HasColumnName("id");
        
        builder.Property(v => v.UserId)
            .HasColumnName("user_id");
    }
}