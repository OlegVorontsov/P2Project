using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using P2Project.Application.Shared.Dtos.Pets;

namespace P2Project.Infrastructure.Configurations.Read;

public class BreedReadDtoConfiguration : IEntityTypeConfiguration<BreedReadDto>
{
    public void Configure(EntityTypeBuilder<BreedReadDto> builder)
    {
        builder.ToTable("breeds");

        builder.HasKey(b => b.Id);
    }
}