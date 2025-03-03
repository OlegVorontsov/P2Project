using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using P2Project.Core.Dtos.Common;
using P2Project.Core.Extensions;
using P2Project.SharedKernel;
using P2Project.SharedKernel.IDs;
using P2Project.SharedKernel.ValueObjects;
using P2Project.Volunteers.Domain.Entities;
using P2Project.Volunteers.Domain.ValueObjects.Pets;

namespace P2Project.Volunteers.Infrastructure.Configurations.Write
{
    public class PetDtoConfiguration : IEntityTypeConfiguration<Pet>
    {
        public void Configure(EntityTypeBuilder<Pet> builder)
        {
            builder.ToTable(Pet.DB_TABLE_PETS);

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
                  .HasColumnName(NickName.DB_COLUMN_NICKNAME);
            });

            builder.ComplexProperty(p => p.SpeciesBreed, sbb =>
            {
                sbb.Property(si => si.SpeciesId)
                   .HasConversion(
                        id => id.Value,
                        value => SpeciesId.Create(value))
                   .HasColumnName(SpeciesBreed.DB_COLUMN_SPECIES_ID);

                sbb.Property(bi => bi.BreedId)
                   .HasColumnName(SpeciesBreed.DB_COLUMN_BREED_ID);
            });

            builder.ComplexProperty(p => p.Description, db =>
            {
                db.Property(d => d.Value)
                  .IsRequired(false)
                  .HasMaxLength(Constants.MAX_BIG_TEXT_LENGTH)
                  .HasColumnName(Description.DB_COLUMN_DESCRIPTION);
            });

            builder.ComplexProperty(p => p.Color, cb =>
            {
                cb.Property(c => c.Value)
                  .IsRequired()
                  .HasMaxLength(Constants.MAX_SMALL_TEXT_LENGTH)
                  .HasColumnName(Color.DB_COLUMN_COLOR);
            });

            builder.ComplexProperty(p => p.HealthInfo, hib =>
            {
                hib.Property(pp => pp.Weight)
                    .IsRequired()
                    .HasColumnName(HealthInfo.DB_COLUMN_WEIGHT);
                
                hib.Property(pp => pp.Height)
                    .IsRequired()
                    .HasColumnName(HealthInfo.DB_COLUMN_HEIGHT);

                hib.Property(pp => pp.IsCastrated)
                    .IsRequired()
                    .HasColumnName(HealthInfo.DB_COLUMN_IS_CASTRATED);
                
                hib.Property(pp => pp.IsVaccinated)
                    .IsRequired()
                    .HasColumnName(HealthInfo.DB_COLUMN_IS_VACCINATED);
                
                hib.Property(hi => hi.HealthDescription)
                  .IsRequired(false)
                  .HasMaxLength(Constants.MAX_MEDIUM_TEXT_LENGTH)
                  .HasColumnName(HealthInfo.DB_COLUMN_HEALTH_DESCRIPTION);
            });

            builder.ComplexProperty(p => p.Address, ab =>
            {
                ab.Property(a => a.Region)
                  .IsRequired()
                  .HasMaxLength(Constants.MAX_SMALL_TEXT_LENGTH)
                  .HasColumnName(Address.DB_COLUMN_REGION);

                ab.Property(a => a.City)
                  .IsRequired()
                  .HasMaxLength(Constants.MAX_SMALL_TEXT_LENGTH)
                  .HasColumnName(Address.DB_COLUMN_CITY);

                ab.Property(a => a.Street)
                  .IsRequired()
                  .HasMaxLength(Constants.MAX_SMALL_TEXT_LENGTH)
                  .HasColumnName(Address.DB_COLUMN_STREET);

                ab.Property(a => a.House)
                  .IsRequired()
                  .HasMaxLength(Constants.MAX_SMALL_TEXT_LENGTH)
                  .HasColumnName(Address.DB_COLUMN_HOUSE);

                ab.Property(a => a.Floor)
                  .IsRequired(false)
                  .HasMaxLength(Constants.MAX_SMALL_TEXT_LENGTH)
                  .HasColumnName(Address.DB_COLUMN_FLOOR);

                ab.Property(a => a.Apartment)
                  .IsRequired(false)
                  .HasMaxLength(Constants.MAX_SMALL_TEXT_LENGTH)
                  .HasColumnName(Address.DB_COLUMN_APARTMENT);
            });

            builder.ComplexProperty(p => p.PhoneNumber, pnb =>
            {
                pnb.Property(pn => pn.Value)
                   .IsRequired()
                   .HasMaxLength(Constants.MAX_SMALL_TEXT_LENGTH)
                   .HasColumnName(PhoneNumber.DB_COLUMN_PHONE_NUMBER);

                pnb.Property(pn => pn.IsMain)
                   .IsRequired(false)
                   .HasColumnName(PhoneNumber.DB_COLUMN_IS_MAIN);
            });

            builder.Property(p => p.BirthDate)
                .IsRequired()
                .HasConversion(
                    d => d.ToShortDateString(),
                    d => DateOnly.Parse(d))
                .HasColumnName(Pet.DB_COLUMN_BIRTH_DATE);

            builder.ComplexProperty(p => p.AssistanceStatus, asb =>
            {
                asb.Property(a => a.Status)
                   .IsRequired()
                   .HasColumnName(AssistanceStatus.DB_COLUMN_ASSISTANCE_STATUS);
            });
            
            builder.Property(p => p.AssistanceDetails)
                .ValueObjectsCollectionJsonConversion(
                    detail => new AssistanceDetailDto(
                        detail.Name, detail.Description, detail.AccountNumber),
                    dto => AssistanceDetail.Create(
                        dto.Name, dto.Description, dto.AccountNumber).Value)
                .HasColumnName(Pet.DB_COLUMN_ASSISTANCE_DETAILS);
            
            builder.Property(p => p.Photos)
                .ValueObjectsCollectionJsonConversion(
                    photo => new PhotoDto(photo.FilePath, photo.IsMain),
                    dto => Photo.Create(dto.Path, dto.IsMain).Value)
                .HasColumnName(Pet.DB_COLUMN_PHOTOS);

            builder.Property(p => p.CreatedAt)
                .IsRequired()
                .HasConversion(
                    d => d.ToShortDateString(),
                    d => DateOnly.Parse(d))
                .HasColumnName(Pet.DB_COLUMN_CREATED_AT);

            builder.ComplexProperty(p => p.Position, snb =>
            {
                snb.Property(sn => sn.Value)
                  .IsRequired()
                  .HasColumnName(Position.DB_COLUMN_POSITION);
            });
            
            builder.Property(v => v.IsDeleted)
                .HasColumnName("is_deleted")
                .IsRequired();

            builder.Property(v => v.DeletionDateTime)
                .HasColumnName("deletion_datetime");

            builder.Property(p => p.VolunteerId)
                .IsRequired();
        }
    }
}
