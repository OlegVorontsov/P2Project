using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using P2Project.Application.Shared.Dtos;

namespace P2Project.Infrastructure.Configurations.Read
{
    public class PetDtoConfiguration : IEntityTypeConfiguration<PetDto>
    {
        public void Configure(EntityTypeBuilder<PetDto> builder)
        {
            builder.ToTable("pets");

            builder.HasKey(p => p.Id);
            
            builder.Property(p => p.PetPhotosDto)
                .HasConversion(
                    photos => JsonSerializer
                        .Serialize(string.Empty, JsonSerializerOptions.Default),
                    
                    json => JsonSerializer.Deserialize<PetPhotoDto[]>(json, JsonSerializerOptions.Default)!);
        }
    }
}
