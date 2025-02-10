using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using P2Project.Core.Dtos.Accounts;

namespace P2Project.Accounts.Infrastructure.Configurations.Read;

public class ParticipantAccountDtoConfiguration :
    IEntityTypeConfiguration<ParticipantAccountDto>
{
    public void Configure(EntityTypeBuilder<ParticipantAccountDto> builder)
    {
        builder.ToTable("participant_accounts");
        
        builder.HasKey(v => v.ParticipantAccountId);
       
        builder.Property(x => x.ParticipantAccountId)
            .HasColumnName("id");
        
        builder.Property(v => v.UserId)
            .HasColumnName("user_id");
    }
}