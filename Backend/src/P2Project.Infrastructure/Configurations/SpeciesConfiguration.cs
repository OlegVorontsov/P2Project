using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using P2Project.Domain.Shared;
using P2Project.Domain.Shared.IDs;
using P2Project.Domain.SpeciesManagment;

namespace P2Project.Infrastructure.Configurations
{
    public class SpeciesConfiguration : IEntityTypeConfiguration<Species>
    {
        public void Configure(EntityTypeBuilder<Species> builder)
        {
            builder.ToTable("species");

            builder.HasKey(s => s.Id);

            builder.Property(s => s.Id)
                   .HasConversion(
                        id => id.Value,
                        value => SpeciesId.CreateSpeciesId(value));

            builder.ComplexProperty(s => s.Name, snb =>
            {
                snb.Property(sn => sn.Value)
                   .IsRequired()
                   .HasMaxLength(Constants.MAX_SMALL_TEXT_LENGTH)
                   .HasColumnName("name");
            });

            builder.HasMany(s => s.Breeds)
                   .WithOne()
                   .HasForeignKey("species_id");
        }
    }
}
