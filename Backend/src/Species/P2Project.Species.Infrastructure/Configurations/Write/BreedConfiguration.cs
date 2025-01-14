using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using P2Project.Core;
using P2Project.SharedKernel;
using P2Project.SharedKernel.IDs;
using P2Project.Species.Domain.Entities;

namespace P2Project.Species.Infrastructure.Configurations.Write
{
    public class BreedConfiguration : IEntityTypeConfiguration<Breed>
    {
        public void Configure(EntityTypeBuilder<Breed> builder)
        {
            builder.ToTable("breeds");
            
            builder.HasKey(b => b.Id);
            
            builder.Property(x => x.Id)
                .HasConversion(
                    id => id.Value,
                    value => BreedId.Create(value));

            builder.ComplexProperty(b => b.Name, nb =>
            {
                nb.Property(n => n.Value)
                  .IsRequired()
                  .HasMaxLength(Constants.MAX_SMALL_TEXT_LENGTH)
                  .HasColumnName("name");
            });
        }
    }
}
