using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using P2Project.Discussions.Domain.Entities;
using P2Project.SharedKernel.ValueObjects;

namespace P2Project.Discussions.Infrastructure.Configurations.Write;

public class MessageConfiguration : IEntityTypeConfiguration<Message>
{
    public void Configure(EntityTypeBuilder<Message> builder)
    {
        builder.ToTable("messages");
        
        builder.HasKey(b => b.Id);
        
        builder.Property(v => v.Content)
            .HasConversion(
                id => id.Value,
                value => Content.Create(value).Value)
            .IsRequired()
            .HasColumnName("message_content");
        
        builder.Property(s => s.SenderId)
            .IsRequired()
            .HasColumnName("sender_id");
        
        builder.Property(s => s.CreatedAt)
            .IsRequired()
            .HasColumnName("created_at");
        
        builder.Property(s => s.IsEdited)
            .IsRequired()
            .HasColumnName("is_edited");
    }
}