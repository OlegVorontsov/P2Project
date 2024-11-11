using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using P2Project.Domain.Models;
using P2Project.Domain.Shared;

namespace P2Project.Infrastructure.Configurations
{
    public class BreedConfiguration : IEntityTypeConfiguration<Breed>
    {
        public void Configure(EntityTypeBuilder<Breed> builder)
        {
            builder.HasKey(b => b.BreedId);

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
