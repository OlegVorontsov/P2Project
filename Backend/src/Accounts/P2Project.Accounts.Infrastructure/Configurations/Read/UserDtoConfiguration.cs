using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using P2Project.Core.Dtos.Accounts;
using P2Project.Core.Dtos.Common;
using P2Project.Core.Dtos.Volunteers;
using P2Project.Volunteers.Domain;
using P2Project.Volunteers.Domain.Entities;

namespace P2Project.Accounts.Infrastructure.Configurations.Read;

public class UserDtoConfiguration : IEntityTypeConfiguration<UserDto>
{
    public void Configure(EntityTypeBuilder<UserDto> builder)
    {
        builder.ToTable("users");

        builder.HasKey(v => v.Id)
            .HasName("id");
        
        builder.Property(u => u.FirstName)
            .HasColumnName("first_name");
        
        builder.Property(u => u.SecondName)
            .HasColumnName("second_name");
        
        builder.Property(u => u.LastName)
            .HasColumnName("last_name");
        
        builder.Property(u => u.UserName)
            .HasColumnName("user_name");

        builder.Property(u => u.Email)
            .HasColumnName("email");
        
        builder.Property(x => x.SocialNetworks)
            .HasConversion(
                values => JsonSerializer
                    .Serialize(string.Empty, JsonSerializerOptions.Default),
                json => JsonSerializer
                    .Deserialize<IEnumerable<SocialNetworkDto>>(
                        json, JsonSerializerOptions.Default)!)
            .HasColumnName("social_networks");
        
        builder.Property(p => p.Photos)
            .HasConversion(
                photos => JsonSerializer
                    .Serialize(string.Empty, JsonSerializerOptions.Default),
                json => JsonSerializer
                    .Deserialize<IEnumerable<PhotoDto>>(
                        json, JsonSerializerOptions.Default)!)
            .HasColumnName("photos");

        builder.HasOne(u => u.AdminAccount)
            .WithOne()
            .HasForeignKey<AdminAccountDto>(a => a.UserId);

        builder.HasOne(u => u.VolunteerAccount)
            .WithOne()
            .HasForeignKey<VolunteerAccountDto>(v => v.UserId);
        
        builder.HasOne(u => u.ParticipantAccount)
            .WithOne()
            .HasForeignKey<ParticipantAccountDto>(p => p.UserId);
    }
}