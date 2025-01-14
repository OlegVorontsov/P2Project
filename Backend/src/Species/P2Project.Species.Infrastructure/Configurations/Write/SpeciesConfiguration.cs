using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using P2Project.Core;
using P2Project.SharedKernel;
using P2Project.SharedKernel.IDs;

namespace P2Project.Species.Infrastructure.Configurations.Write
{
    public class SpeciesConfiguration : IEntityTypeConfiguration<Domain.Species>
    {
        public void Configure(EntityTypeBuilder<Domain.Species> builder)
        {
            builder.ToTable("species");

            builder.HasKey(s => s.Id);

            builder.Property(s => s.Id)
                   .HasConversion(
                        id => id.Value,
                        value => SpeciesId.Create(value));

            builder.ComplexProperty(s => s.Name, snb =>
            {
                snb.Property(sn => sn.Value)
                   .IsRequired()
                   .HasMaxLength(Constants.MAX_SMALL_TEXT_LENGTH)
                   .HasColumnName("name");
            });

            builder.HasMany(s => s.Breeds)
                   .WithOne()
                   .OnDelete(DeleteBehavior.Cascade)
                   .HasForeignKey(b => b.SpeciesId);
        }
    }
}
