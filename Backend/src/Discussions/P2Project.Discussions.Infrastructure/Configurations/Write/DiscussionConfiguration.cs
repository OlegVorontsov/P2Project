using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using P2Project.Discussions.Domain;

namespace P2Project.Discussions.Infrastructure.Configurations.Write;

public class DiscussionConfiguration :
    IEntityTypeConfiguration<Discussion>
{
    public void Configure(EntityTypeBuilder<Discussion> builder)
    {
        builder.ToTable("discussions");
        
        builder.HasKey(b => b.Id);
        
        builder.Property(s => s.Id)
            .IsRequired()
            .HasColumnName("id");
        
        builder.Property(s => s.RequestId)
            .IsRequired()
            .HasColumnName("request_id");
        
        builder.HasMany(d => d.Messages)
            .WithOne()
            .HasForeignKey(m => m.DiscussionId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.ComplexProperty(p => p.DiscussionUsers, du =>
        {
            du.Property(p => p.ReviewingUserId)
                .IsRequired()
                .HasColumnName("reviewing_user_id");
            
            du.Property(p => p.ApplicantUserId)
                .IsRequired()
                .HasColumnName("applicant_user_id");
        });
        
        builder.Property(v=> v.Status)
            .HasConversion<string>()
            .HasColumnName("status")
            .IsRequired();
    }
}