using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using P2Project.Core.Dtos.Accounts;
using P2Project.Core.Dtos.Common;
using P2Project.Volunteers.Domain.Entities;

namespace P2Project.Accounts.Infrastructure.Configurations.Read;

public class VolunteerAccountDtoConfiguration :
    IEntityTypeConfiguration<VolunteerAccountDto>
{
    public void Configure(EntityTypeBuilder<VolunteerAccountDto> builder)
    {
        builder.ToTable("volunteer_accounts");
        
        builder.HasKey(v => v.VolunteerAccountId);
        
        builder.Property(x => x.VolunteerAccountId)
            .HasColumnName("id");
        
        builder.Property(v => v.UserId)
            .HasColumnName("user_id");
        
        builder.Property(v => v.Experience)
            .HasColumnName("experience");
        
        builder.Property(p => p.AssistanceDetails)
            .HasConversion(
                detail => JsonSerializer
                    .Serialize(string.Empty, JsonSerializerOptions.Default),
                json => JsonSerializer
                    .Deserialize<IEnumerable<AssistanceDetailDto>>(
                        json, JsonSerializerOptions.Default)!)
            .HasColumnName("assistance_details");
        
        builder.Property(p => p.Certificates)
            .HasConversion(
                detail => JsonSerializer
                    .Serialize(string.Empty, JsonSerializerOptions.Default),
                json => JsonSerializer
                    .Deserialize<IEnumerable<CertificateDto>>(
                        json, JsonSerializerOptions.Default)!)
            .HasColumnName("certificates");
    }
}