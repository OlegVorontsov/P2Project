using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using P2Project.Core.Dtos.Common;
using P2Project.Core.Dtos.Volunteers;
using P2Project.Volunteers.Domain;

namespace P2Project.Volunteers.Infrastructure.Configurations.Read
{
    public class VolunteerDtoConfiguration : IEntityTypeConfiguration<VolunteerDto>
    {
        public void Configure(EntityTypeBuilder<VolunteerDto> builder)
        {
            builder.ToTable(Volunteer.DB_TABLE_VOLUNTEERS);

            builder.HasKey(v => v.Id);

            builder.HasMany(v => v.Pets)
                   .WithOne()
                   .HasForeignKey(p => p.VolunteerId);
            
            builder.Property(x => x.PhoneNumbers)
                .HasConversion(
                    values => JsonSerializer
                        .Serialize(string.Empty, JsonSerializerOptions.Default),
                    json => JsonSerializer
                        .Deserialize<IEnumerable<PhoneNumberDto>>(
                            json, JsonSerializerOptions.Default)!)
                .HasColumnName(Volunteer.DB_COLUMN_PHONE_NUMBERS);
        }
    }
}
