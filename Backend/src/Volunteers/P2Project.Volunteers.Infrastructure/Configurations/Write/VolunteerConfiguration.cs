using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using P2Project.Core.Dtos.Common;
using P2Project.Core.Dtos.Volunteers;
using P2Project.Core.Extensions;
using P2Project.SharedKernel;
using P2Project.SharedKernel.IDs;
using P2Project.SharedKernel.ValueObjects;
using P2Project.Volunteers.Domain;

namespace P2Project.Volunteers.Infrastructure.Configurations.Write
{
    public class VolunteerDtoConfiguration : IEntityTypeConfiguration<Volunteer>
    {
        public void Configure(EntityTypeBuilder<Volunteer> builder)
        {
            builder.ToTable(Volunteer.DB_TABLE_VOLUNTEERS);

            builder.HasKey(v => v.Id);
            builder.Property(v => v.Id)
                    .HasConversion(
                    id => id.Value,
                    value => VolunteerId.Create(value));

            builder.ComplexProperty(v => v.VolunteerInfo, vib =>
            {
                vib.Property(vi => vi.Age)
                    .IsRequired()
                    .HasColumnName(VolunteerInfo.DB_COLUMN_AGE);

                vib.Property(vi => vi.Grade)
                    .IsRequired()
                    .HasColumnName(VolunteerInfo.DB_COLUMN_GRADE);
            });

            builder.Property(v => v.Gender)
                   .IsRequired()
                   .HasConversion<string>()
                   .HasColumnName(Volunteer.DB_COLUMN_GENDER);

            builder.ComplexProperty(v => v.Description, db =>
            {
                db.Property(d => d.Value)
                  .IsRequired(false)
                  .HasMaxLength(Constants.MAX_BIG_TEXT_LENGTH)
                  .HasColumnName(Description.DB_COLUMN_DESCRIPTION);
            });

            builder.Property(v => v.RegisteredAt)
                .IsRequired()
                .HasColumnName(Volunteer.DB_COLUMN_REGISTERED_AT);

            builder.HasMany(v => v.Pets)
                   .WithOne()
                   .HasForeignKey(p => p.VolunteerId)
                   .IsRequired();

            builder.Property(v => v.NeedsHelpPets)
                   .HasColumnName("needs_help_pets");
            
            builder.Property(v => v.NeedsFoodPets)
                .HasColumnName("needs_food_pets");
            
            builder.Property(v => v.OnMedicationPets)
                .HasColumnName("on_medication_pets");
            
            builder.Property(v => v.LooksForHomePets)
                .HasColumnName("looks_for_home_pets");
            
            builder.Property(v => v.FoundHomePets)
                .HasColumnName("found_home_pets");
            
            builder.Property(v => v.UnknownStatusPets)
                .HasColumnName("unknown_status_pets");

            builder.Property(v => v.PhoneNumbers)
                .ValueObjectsCollectionJsonConversion(
                    phone => new PhoneNumberDto(
                        phone.Value, phone.IsMain),
                    dto => PhoneNumber.Create(
                        dto.Value, dto.IsMain).Value)
                .HasColumnName(Volunteer.DB_COLUMN_PHONE_NUMBERS);
            
            builder.Property(v => v.IsDeleted)
                .HasColumnName("is_deleted")
                .IsRequired();

            builder.Property(v => v.DeletionDateTime)
                .HasColumnName("deletion_datetime");
        }
    }
}
