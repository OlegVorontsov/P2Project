using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using P2Project.Domain.IDs;
using P2Project.Domain.Models;
using P2Project.Domain.Shared;

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

            builder.Property(v => v.FirstName)
                   .IsRequired()
                   .HasMaxLength(Constants.MAX_SMALL_TEXT_LENGTH);

            builder.Property(v => v.SecondName)
                   .IsRequired()
                   .HasMaxLength(Constants.MAX_SMALL_TEXT_LENGTH);

            builder.Property(v => v.LastName)
                   .IsRequired()
                   .HasMaxLength(Constants.MAX_SMALL_TEXT_LENGTH);

            builder.Property(v => v.Age)
                   .IsRequired()
                   .HasMaxLength(Constants.MAX_TINY_TEXT_LENGTH);

            // enum Gender

            builder.Property(v => v.Email)
                   .IsRequired()
                   .HasMaxLength(Constants.MAX_MEDIUM_TEXT_LENGTH);

            builder.Property(v => v.Description)
                   .IsRequired()
                   .HasMaxLength(Constants.MAX_BIG_TEXT_LENGTH);

            // DateTime RegisteredDate RegisteredDate

            // double YearsOfExperience

            builder.HasMany(v => v.Pets)
                   .WithOne()
                   .HasForeignKey("volunteer_id");

            // int NeedsHelpPet

            // int NeedsFoodPets

            // int OnMedicationPets

            // int LooksForHomePets

            // int FoundHomePets

            // string PhoneNumber

            // IReadOnlyList<SocialNetwork> SocialNetworks

            // IReadOnlyList<AssistanceDetail> AssistanceDetails
        }
    }
}
