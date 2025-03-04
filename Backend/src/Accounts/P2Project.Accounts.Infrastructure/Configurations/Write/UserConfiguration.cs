using System.Text.Json;
using FilesService.Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using P2Project.Accounts.Domain;
using P2Project.Accounts.Domain.Users.ValueObjects;
using P2Project.Core.Dtos.Common;
using P2Project.Core.Extensions;
using P2Project.SharedKernel;
using P2Project.SharedKernel.ValueObjects;

namespace P2Project.Accounts.Infrastructure.Configurations.Write;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");
        
        builder.ComplexProperty(v => v.FullName, fnb =>
        {
            fnb.Property(fn => fn.FirstName)
                .IsRequired()
                .HasMaxLength(Constants.MAX_SMALL_TEXT_LENGTH)
                .HasColumnName(FullName.DB_COLUMN_FIRST_NAME);

            fnb.Property(snb => snb.SecondName)
                .IsRequired()
                .HasMaxLength(Constants.MAX_SMALL_TEXT_LENGTH)
                .HasColumnName(FullName.DB_COLUMN_SECOND_NAME);

            fnb.Property(lnb => lnb.LastName)
                .IsRequired(false)
                .HasMaxLength(Constants.MAX_SMALL_TEXT_LENGTH)
                .HasColumnName(FullName.DB_COLUMN_LAST_NAME);
        });
        
        builder.Property(u => u.SocialNetworks)
            .HasConversion(
                u => JsonSerializer
                    .Serialize(u, JsonSerializerOptions.Default),
                json => JsonSerializer
                    .Deserialize<IReadOnlyList<SocialNetwork>>(json, JsonSerializerOptions.Default)!);
        
        builder.Property(u => u.Photos)
            .ValueObjectsCollectionJsonConversion(
                photo => new MediaFileDto(photo.BucketName, photo.FileName, photo.IsMain),
                dto => MediaFile.Create(dto.BucketName, dto.FileName, dto.IsMain).Value)
            .HasColumnName("photos");
        
        builder.Ignore(u => u.PhotosUrls);

        builder.OwnsOne(u => u.Avatar, ab =>
        {
            ab.ToJson("avatar");
            
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

            ab.Property(a => a.IsMain)
                .IsRequired(false)
                .HasColumnName("is_main");
        });
        
        builder.Ignore(u => u.AvatarUrl);

        builder.HasMany(u => u.Roles)
            .WithMany()
            .UsingEntity<IdentityUserRole<Guid>>();
    }
}