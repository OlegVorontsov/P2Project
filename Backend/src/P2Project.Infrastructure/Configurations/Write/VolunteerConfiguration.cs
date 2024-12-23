using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using P2Project.Application.Shared.Dtos.Common;
using P2Project.Application.Shared.Dtos.Volunteers;
using P2Project.Domain.Extensions;
using P2Project.Domain.PetManagment;
using P2Project.Domain.PetManagment.ValueObjects.Common;
using P2Project.Domain.PetManagment.ValueObjects.Volunteers;
using P2Project.Domain.Shared;
using P2Project.Domain.Shared.IDs;
using P2Project.Infrastructure.Extensions;

namespace P2Project.Infrastructure.Configurations.Write
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

            builder.ComplexProperty(v => v.Email, eb =>
            {
                eb.Property(e => e.Value)
                  .IsRequired()
                  .HasMaxLength(Constants.MAX_MEDIUM_TEXT_LENGTH)
                  .HasColumnName(Email.DB_COLUMN_EMAIL);
            });

            builder.ComplexProperty(v => v.Description, db =>
            {
                db.Property(d => d.Value)
                  .IsRequired(false)
                  .HasMaxLength(Constants.MAX_BIG_TEXT_LENGTH)
                  .HasColumnName(Description.DB_COLUMN_DESCRIPTION);
            });

            builder.Property(v => v.RegisteredAt)
                .IsRequired()
                .SetLocalDateTime(DateTimeKind.Local)
                .HasColumnName(Volunteer.DB_COLUMN_REGISTERED_AT);

            builder.Property(v => v.YearsOfExperience)
                   .HasColumnName(Volunteer.DB_COLUMN_YEARS_OF_EXPERIENCE);

            builder.HasMany(v => v.Pets)
                   .WithOne()
                   .HasForeignKey(p => p.VolunteerId)
                   .OnDelete(DeleteBehavior.NoAction);

            builder.Property(v => v.PhoneNumbers)
                .ValueObjectsCollectionJsonConversion(
                    phone => new PhoneNumberDto(
                        phone.Value, phone.IsMain),
                    dto => PhoneNumber.Create(
                        dto.Value, dto.IsMain).Value)
                .HasColumnName(Volunteer.DB_COLUMN_PHONE_NUMBERS);

            builder.Property(v => v.SocialNetworks)
                .ValueObjectsCollectionJsonConversion(
                    social => new SocialNetworkDto(
                        social.Name, social.Link),
                    dto => SocialNetwork.Create(
                        dto.Name, dto.Link).Value)
                .HasColumnName(Volunteer.DB_COLUMN_SOCIAL_NETWORKS);

            builder.Property(v => v.AssistanceDetails)
                .ValueObjectsCollectionJsonConversion(
                    detail => new AssistanceDetailDto(
                        detail.Name, detail.Description, detail.AccountNumber),
                    dto => AssistanceDetail.Create(
                        dto.Name, dto.Description, dto.AccountNumber).Value)
                .HasColumnName(Volunteer.DB_COLUMN_ASSISTANCE_DETAILS);
            
            builder.Property(v => v.IsDeleted)
                .HasColumnName("is_deleted")
                .IsRequired();

            builder.Property(v => v.DeletionDateTime)
                .HasColumnName("deletion_datetime");
        }
    }
}
