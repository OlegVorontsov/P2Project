using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using P2Project.Accounts.Domain.Accounts;
using P2Project.SharedKernel.ValueObjects;

namespace P2Project.Accounts.Infrastructure.Configurations.Write;

public class VolunteerAccountConfiguration :
    IEntityTypeConfiguration<VolunteerAccount>
{
    public void Configure(EntityTypeBuilder<VolunteerAccount> builder)
    {
        builder.ToTable("volunteer_accounts");
        builder.HasKey(x => x.Id);
        
        builder.ComplexProperty(v => v.Experience, eb =>
        {
            eb.Property(p => p.Value)
                .HasColumnName("experience")
                .IsRequired();
        });
        
        builder.Property(u => u.AssistanceDetails)
            .HasConversion(
                u => JsonSerializer
                    .Serialize(u, JsonSerializerOptions.Default),
                json => JsonSerializer
                    .Deserialize<IReadOnlyList<AssistanceDetail>>(json, JsonSerializerOptions.Default)!);
        
        builder.Property(u => u.Certificates)
            .HasConversion(
                u => JsonSerializer
                    .Serialize(u, JsonSerializerOptions.Default),
                json => JsonSerializer
                    .Deserialize<IReadOnlyList<Certificate>>(json, JsonSerializerOptions.Default)!);
    }
}