using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using P2Project.Core.Dtos.Common;
using P2Project.Core.Dtos.Pets;
using P2Project.Volunteers.Domain.Entities;

namespace P2Project.Volunteers.Infrastructure.Configurations.Read
{
    public class PetDtoConfiguration : IEntityTypeConfiguration<PetDto>
    {
        public void Configure(EntityTypeBuilder<PetDto> builder)
        {
            builder.ToTable(Pet.DB_TABLE_PETS);

            builder.HasKey(p => p.Id);

            builder.Property(p => p.AssistanceDetails)
                .HasConversion(
                    detail => JsonSerializer
                        .Serialize(string.Empty, JsonSerializerOptions.Default),
                    json => JsonSerializer
                        .Deserialize<IEnumerable<AssistanceDetailDto>>(
                            json, JsonSerializerOptions.Default)!)
                .HasColumnName(Pet.DB_COLUMN_ASSISTANCE_DETAILS);
            
            builder.Property(p => p.Photos)
                .HasConversion(
                    photos => JsonSerializer
                        .Serialize(string.Empty, JsonSerializerOptions.Default),
                    
                    json => JsonSerializer
                        .Deserialize<IEnumerable<PhotoDto>>(
                            json, JsonSerializerOptions.Default)!)
                .HasColumnName(Pet.DB_COLUMN_PHOTOS);
        }
    }
}
