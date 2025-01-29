using System.Text.Json;
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
        
        builder.Property(p => p.Photos)
            .ValueObjectsCollectionJsonConversion(
                photo => new PhotoDto(photo.FilePath, photo.IsMain),
                dto => Photo.Create(dto.Path, dto.IsMain).Value)
            .HasColumnName("photos");

        builder.HasMany(u => u.Roles)
            .WithMany()
            .UsingEntity<IdentityUserRole<Guid>>();
    }
}