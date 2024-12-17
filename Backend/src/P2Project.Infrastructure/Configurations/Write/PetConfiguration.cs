using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using P2Project.Application.Shared.Dtos;
using P2Project.Domain.PetManagment.Entities;
using P2Project.Domain.PetManagment.ValueObjects;
using P2Project.Domain.Shared;
using P2Project.Domain.Shared.IDs;

namespace P2Project.Infrastructure.Configurations.Write
{
    public class PetDtoConfiguration : IEntityTypeConfiguration<Pet>
    {
        public void Configure(EntityTypeBuilder<Pet> builder)
        {
            builder.ToTable("pets");

            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id)
                    .HasConversion(
                    id => id.Value,
                    value => PetId.Create(value));

            builder.ComplexProperty(p => p.NickName, nb =>
            {
                nb.Property(n => n.Value)
                  .IsRequired()
                  .HasMaxLength(Constants.MAX_SMALL_TEXT_LENGTH)
                  .HasColumnName("nick_name");
            });

            builder.ComplexProperty(p => p.SpeciesBreed, sbb =>
            {
                sbb.Property(si => si.SpeciesId)
                   .HasConversion(
                        id => id.Value,
                        value => SpeciesId.Create(value))
                   .HasColumnName("species_id");

                sbb.Property(bi => bi.BreedId)
                   .HasColumnName("breed_id");
            });

            builder.ComplexProperty(p => p.Description, db =>
            {
                db.Property(d => d.Value)
                  .IsRequired(false)
                  .HasMaxLength(Constants.MAX_BIG_TEXT_LENGTH)
                  .HasColumnName("description");
            });

            builder.ComplexProperty(p => p.Color, cb =>
            {
                cb.Property(c => c.Value)
                  .IsRequired()
                  .HasMaxLength(Constants.MAX_SMALL_TEXT_LENGTH)
                  .HasColumnName("color");
            });

            builder.ComplexProperty(p => p.HealthInfo, hib =>
            {
                hib.Property(hi => hi.Value)
                  .IsRequired(false)
                  .HasMaxLength(Constants.MAX_MEDIUM_TEXT_LENGTH)
                  .HasColumnName("health_info");
            });

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
                  .IsRequired(false)
                  .HasMaxLength(Constants.MAX_SMALL_TEXT_LENGTH);

                ab.Property(a => a.Apartment)
                  .IsRequired(false)
                  .HasMaxLength(Constants.MAX_SMALL_TEXT_LENGTH);
            });

            builder.Property(p => p.Weight)
                   .IsRequired()
                   .HasMaxLength(Constants.MAX_TINY_TEXT_LENGTH)
                   .HasColumnName("weight");

            builder.Property(p => p.Height)
                   .IsRequired()
                   .HasMaxLength(Constants.MAX_TINY_TEXT_LENGTH)
                   .HasColumnName("height");

            builder.ComplexProperty(p => p.OwnerPhoneNumber, pnb =>
            {
                pnb.Property(pn => pn.Value)
                   .IsRequired()
                   .HasMaxLength(Constants.MAX_SMALL_TEXT_LENGTH)
                   .HasColumnName("owner_phone_number");

                pnb.Property(pn => pn.IsMain)
                   .IsRequired(false)
                   .HasColumnName("is_main");
            });

            builder.ComplexProperty(p => p.HealthInfo, hib =>
            {
                hib.Property(hi => hi.Value)
                  .IsRequired(false)
                  .HasMaxLength(Constants.MAX_MEDIUM_TEXT_LENGTH)
                  .HasColumnName("health_info");
            });

            builder.Property(p => p.IsCastrated)
                   .IsRequired()
                   .HasColumnName("is_castrated");

            builder.Property(p => p.IsVaccinated)
                   .IsRequired()
                   .HasColumnName("is_vaccinated");

            builder.Property(p => p.DateOfBirth)
                   .IsRequired()
                   .HasColumnName("date_of_birth")
                   .HasConversion(
                        d => d.ToShortDateString(),
                        d => DateOnly.Parse(d));

            builder.ComplexProperty(p => p.AssistanceStatus, asb =>
            {
                asb.Property(a => a.Status)
                   .IsRequired()
                   .HasColumnName("assistance_status");
            });

            builder.OwnsOne(p => p.AssistanceDetails, adb =>
            {
                adb.ToJson();

                adb.OwnsMany(ad => ad.AssistanceDetails, ab =>
                {
                    ab.Property(a => a.Name)
                      .IsRequired()
                      .HasMaxLength(Constants.MAX_SMALL_TEXT_LENGTH);

                    ab.Property(a => a.Description)
                      .IsRequired()
                      .HasMaxLength(Constants.MAX_MEDIUM_TEXT_LENGTH);

                    ab.Property(a => a.AccountNumber)
                      .IsRequired()
                      .HasMaxLength(Constants.MAX_SMALL_TEXT_LENGTH);
                });
            });
            
            builder.Property(p => p.Photos)
                .HasConversion(
                    photos => JsonSerializer
                        .Serialize(photos
                            .Select(pp =>
                                new PetPhotoDto(pp.FilePath, pp.IsMain)),
                            JsonSerializerOptions.Default),
                    
                    json => JsonSerializer.Deserialize<IEnumerable<PetPhotoDto>>(json, JsonSerializerOptions.Default)!
                        .Select(dto => PetPhoto.Create(dto.Path, dto.IsMain).Value).ToList(),
                    
                    new ValueComparer<IReadOnlyList<PetPhoto>>(
                            (c1, c2) => c1!.SequenceEqual(c2!),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v!.GetHashCode())),
                            c => c.ToList()));
            builder.Property(p => p.Photos).HasColumnType("jsonb");
            builder.Property(p => p.Photos).HasColumnName("photos");

            builder.Property(p => p.CreatedAt)
                   .IsRequired()
                   .HasColumnName("created_at")
                   .HasConversion(
                        d => d.ToShortDateString(),
                        d => DateOnly.Parse(d));

            builder.ComplexProperty(p => p.Position, snb =>
            {
                snb.Property(sn => sn.Value)
                  .IsRequired(true)
                  .HasColumnName("position");
            });
        }
    }
}
