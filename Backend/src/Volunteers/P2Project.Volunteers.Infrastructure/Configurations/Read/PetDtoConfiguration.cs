using System.Text.Json;
using FilesService.Core.Models;
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
            
            builder.Ignore(x => x.AvatarUrl);
            
            builder.ComplexProperty(p => p.Avatar, ab =>
            {
                ab.Property(a => a.Key)
                    .IsRequired()
                    .HasColumnName("key");

                ab.Property(a => a.Type)
                    .HasConversion<string>()
                    .IsRequired()
                    .HasColumnName("type");
                
                ab.Property(a => a.BucketName)
                    .IsRequired()
                    .HasColumnName("bucket_name");

                ab.Property(a => a.FileName)
                    .IsRequired()
                    .HasColumnName("file_name");

                ab.Property(p => p.IsMain)
                    .IsRequired(false)
                    .HasColumnName("is_main");
            });
            
            builder.Ignore(p => p.PhotosUrls);
            
            builder.Property(p => p.Photos)
                .HasConversion(
                    photos => JsonSerializer
                        .Serialize(photos, JsonSerializerOptions.Default),
                    
                    json => JsonSerializer
                        .Deserialize<IReadOnlyList<MediaFileDto>>(
                            json, JsonSerializerOptions.Default)!)
                .HasColumnName(Pet.DB_COLUMN_PHOTOS);
        }
    }
}
