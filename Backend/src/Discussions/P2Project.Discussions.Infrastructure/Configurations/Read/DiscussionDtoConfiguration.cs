using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using P2Project.Core.Dtos.Discussions;

namespace P2Project.Discussions.Infrastructure.Configurations.Read;

public class DiscussionDtoConfiguration :
    IEntityTypeConfiguration<DiscussionDto>
{
    public void Configure(EntityTypeBuilder<DiscussionDto> builder)
    {
        builder.ToTable("discussions");
        
        builder.HasKey(d => d.Id);
        
        builder.Property(d => d.Id)
            .HasColumnName("discussion_id");
        
        builder.HasMany(d => d.Messages)
            .WithOne()
            .HasForeignKey(m => m.DiscussionId);
    }
}