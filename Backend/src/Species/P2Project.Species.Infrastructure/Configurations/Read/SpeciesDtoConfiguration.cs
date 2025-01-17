using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using P2Project.Core.Dtos.Pets;

namespace P2Project.Species.Infrastructure.Configurations.Read;

public class SpeciesDtoConfiguration : IEntityTypeConfiguration<SpeciesDto>
{
    public void Configure(EntityTypeBuilder<SpeciesDto> builder)
    {
        builder.ToTable("species");
        
        builder.HasKey(s => s.Id);
        
        builder.HasMany(s => s.Breeds)
            .WithOne()
            .HasForeignKey(b => b.SpeciesId);
    }
}