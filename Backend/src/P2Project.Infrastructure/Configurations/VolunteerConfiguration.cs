using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using P2Project.Domain.Extensions;
using P2Project.Domain.IDs;
using P2Project.Domain.Models;
using P2Project.Domain.Shared;
using System.ComponentModel;

namespace P2Project.Infrastructure.Configurations
{
    public class VolunteerConfiguration : IEntityTypeConfiguration<Volunteer>
    {
        public void Configure(EntityTypeBuilder<Volunteer> builder)
        {
            builder.ToTable("volunteers");
            
            builder.HasKey(v => v.Id);
            
            builder.Property(v => v.Id)
                    .HasConversion(
                    id => id.Value,
                    value => VolunteerId.CreateVolunteerId(value));

            builder.ComplexProperty(v => v.FullName, fnb =>
            {
                fnb.Property(fn => fn.FirstName)
                   .IsRequired()
                   .HasMaxLength(Constants.MAX_SMALL_TEXT_LENGTH)
                   .HasColumnName("first_name");

                fnb.Property(fnb => fnb.SecondName)
                   .IsRequired()
                   .HasMaxLength(Constants.MAX_SMALL_TEXT_LENGTH)
                   .HasColumnName("second_name");

                fnb.Property(fnb => fnb.LastName)
                   .IsRequired(false)
                   .HasMaxLength(Constants.MAX_SMALL_TEXT_LENGTH)
                   .HasColumnName("last_name");
            });

            builder.Property(v => v.Age)
                   .IsRequired()
                   .HasMaxLength(Constants.MAX_TINY_TEXT_LENGTH);

            builder.Property(v => v.Gender)
                   .IsRequired()
                   .HasConversion<string>()
                   .HasColumnName("gender");

            builder.ComplexProperty(v => v.Email, eb =>
            {
                eb.Property(e => e.Value)
                  .IsRequired()
                  .HasMaxLength(Constants.MAX_MEDIUM_TEXT_LENGTH)
                  .HasColumnName("email");
            });

            builder.ComplexProperty(v => v.Description, db =>
            {
                db.Property(d => d.Value)
                  .IsRequired(false)
                  .HasMaxLength(Constants.MAX_BIG_TEXT_LENGTH)
                  .HasColumnName("description");
            });

            builder.Property(v => v.RegisteredDate)
                   .IsRequired()
                   .HasColumnName("registered_date")
                   .SetLocalDateTime(DateTimeKind.Local);

            builder.Property(v => v.YearsOfExperience)
                   .HasColumnName("years_of_experience");

            builder.HasMany(v => v.Pets)
                   .WithOne(p => p.Volunteer)
                   .HasForeignKey("volunteer_id")
                   .OnDelete(DeleteBehavior.Cascade)
                   .IsRequired();
            builder.Navigation(v => v.Pets).AutoInclude();

            builder.Property(v => v.NeedsHelpPets)
                   .HasColumnName("needs_help_pets_count");

            builder.Property(v => v.NeedsHelpPets)
                   .HasColumnName("needs_food_pets_count");

            builder.Property(v => v.NeedsHelpPets)
                   .HasColumnName("on_medication_pets_count");

            builder.Property(v => v.NeedsHelpPets)
                   .HasColumnName("looks_for_home_pets_count");

            builder.Property(v => v.NeedsHelpPets)
                   .HasColumnName("found_home_pets_count");

            builder.OwnsOne(v => v.PhoneNumbers, vb =>
            {
                vb.ToJson();

                vb.OwnsMany(pn => pn.PhoneNumbers, pb =>
                {
                    pb.Property(p => p.Value)
                      .IsRequired(false)
                      .HasMaxLength(Constants.MAX_SMALL_TEXT_LENGTH);

                    pb.Property(p => p.IsMain)
                      .IsRequired();
                });
            });

            builder.OwnsOne(v => v.SocialNetworks, vb =>
            {
                vb.ToJson();

                vb.OwnsMany(sn => sn.SocialNetworks, sb =>
                {
                    sb.Property(p => p.Name)
                      .IsRequired()
                      .HasMaxLength(Constants.MAX_SMALL_TEXT_LENGTH);

                    sb.Property(p => p.Link)
                      .IsRequired()
                      .HasMaxLength(Constants.MAX_SMALL_TEXT_LENGTH);
                });
            });

            builder.OwnsOne(v => v.AssistanceDetails, vb =>
            {
                vb.ToJson();

                vb.OwnsMany(ad => ad.AssistanceDetails, ab =>
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
        }
    }
}
