using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using P2Project.Domain.IDs;
using P2Project.Domain.Models;
using P2Project.Domain.Shared;
using P2Project.Domain.ValueObjects;

namespace P2Project.Infrastructure.Configurations
{
    public class PetConfiguration : IEntityTypeConfiguration<Pet>
    {
        public void Configure(EntityTypeBuilder<Pet> builder)
        {
            builder.ToTable("Pets");

            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id)
                    .HasConversion(
                    id => id.Value,
                    value => PetId.CreatePetId(value));

            builder.Property(p => p.NickName)
                   .IsRequired()
                   .HasMaxLength(Constants.MAX_SMALL_TEXT_LENGTH);

            builder.Property(p => p.Species)
                   .IsRequired()
                   .HasMaxLength(Constants.MAX_SMALL_TEXT_LENGTH);

            builder.Property(p => p.Description)
                   .IsRequired()
                   .HasMaxLength(Constants.MAX_BIG_TEXT_LENGTH);

            builder.Property(p => p.Breed)
                   .IsRequired()
                   .HasMaxLength(Constants.MAX_SMALL_TEXT_LENGTH);

            builder.Property(p => p.Color)
                   .IsRequired()
                   .HasMaxLength(Constants.MAX_SMALL_TEXT_LENGTH);

            builder.Property(p => p.HealthInfo)
                   .IsRequired()
                   .HasMaxLength(Constants.MAX_MEDIUM_TEXT_LENGTH);

            builder.OwnsOne(p => p.Address, ab =>
            {
                ab.ToJson();

                ab.Property(a => a.Region)
                  .IsRequired()
                  .HasMaxLength(Constants.MAX_SMALL_TEXT_LENGTH);

                ab.Property(a => a.City)
                  .IsRequired()
                  .HasMaxLength(Constants.MAX_SMALL_TEXT_LENGTH);

                ab.Property(a => a.Street)
                  .IsRequired()
                  .HasMaxLength(Constants.MAX_SMALL_TEXT_LENGTH);

                ab.Property(a => a.House)
                  .IsRequired()
                  .HasMaxLength(Constants.MAX_SMALL_TEXT_LENGTH);

                ab.Property(a => a.Floor)
                  .IsRequired()
                  .HasMaxLength(Constants.MAX_SMALL_TEXT_LENGTH);

                ab.Property(a => a.Apartment)
                  .IsRequired()
                  .HasMaxLength(Constants.MAX_SMALL_TEXT_LENGTH);
            });

            // double Weight

            // double Height

            builder.Property(p => p.OwnerPhoneNumber)
                   .IsRequired()
                   .HasMaxLength(Constants.MAX_SMALL_TEXT_LENGTH);

            builder.Property(p => p.IsCastrated)
                   .HasColumnName("is_castrated");

            // bool IsVaccinated

            // DateTime DateOfBirth

            // AssistanceStatus Status

            // IReadOnlyList<AssistanceDetail> AssistanceDetails

            // IReadOnlyList<PetPhoto> PetPhotos => _petPhotos;

            // DateTime CreatedAt
        }
    }
}
