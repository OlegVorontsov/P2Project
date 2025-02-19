using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using P2Project.Core.Dtos.Discussions;

namespace P2Project.Discussions.Infrastructure.Configurations.Read;

public class MessageDtoConfiguration :
    IEntityTypeConfiguration<MessageDto>
{
    public void Configure(EntityTypeBuilder<MessageDto> builder)
    {
        builder.ToTable("messages");
        
        builder.HasKey(m => m.Id);
        
        builder.Property(m => m.Content)
            .HasColumnName("message_content");
        
        builder.Property(m => m.SenderId)
            .HasColumnName("sender_id");
        
        builder.Property(m => m.CreatedAt)
            .HasColumnName("created_at");
        
        builder.Property(m => m.IsEdited)
            .HasColumnName("is_edited");
    }
}