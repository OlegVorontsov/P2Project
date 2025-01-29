using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using P2Project.Accounts.Domain.Accounts;

namespace P2Project.Accounts.Infrastructure.Configurations.Write;

public class AdminAccountConfiguration : IEntityTypeConfiguration<AdminAccount>
{
    public void Configure(EntityTypeBuilder<AdminAccount> builder)
    {
        builder.ToTable("admin_accounts");
        builder.HasKey(x => x.Id);
    }
}