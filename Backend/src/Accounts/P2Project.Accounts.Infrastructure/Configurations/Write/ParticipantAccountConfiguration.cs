using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using P2Project.Accounts.Domain.Accounts;

namespace P2Project.Accounts.Infrastructure.Configurations.Write;

public class ParticipantAccountConfiguration :
    IEntityTypeConfiguration<ParticipantAccount>
{
    public void Configure(EntityTypeBuilder<ParticipantAccount> builder)
    {
        builder.ToTable("participant_accounts");
        builder.HasKey(x => x.Id);
    }
}