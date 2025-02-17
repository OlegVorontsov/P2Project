using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using P2Project.Core.Dtos.VolunteerRequests;

namespace P2Project.VolunteerRequests.Infrastructure.Configurations.Read;

public class VolunteerRequestDtoConfiguration :
    IEntityTypeConfiguration<VolunteerRequestDto>
{
    public void Configure(EntityTypeBuilder<VolunteerRequestDto> builder)
    {
        builder.ToTable("volunteer_requests");
        
        builder.HasKey(b => b.RequestId);
        
        builder.Property(s => s.RequestId)
            .IsRequired()
            .HasColumnName("request_Id");
        
        builder.Property(s => s.AdminId)
            .IsRequired(false)
            .HasColumnName("admin_Id");
        
        builder.Property(s => s.UserId)
            .HasColumnName("user_Id");
        
        builder.Property(v=> v.FirstName)
            .HasConversion<string>()
            .HasColumnName("first_name")
            .IsRequired();
        
        builder.Property(v=> v.SecondName)
            .HasConversion<string>()
            .HasColumnName("second_name")
            .IsRequired();
        
        builder.Property(v=> v.LastName)
            .HasConversion<string>()
            .HasColumnName("last_name");
        
        builder.Property(s => s.DiscussionId)
            .IsRequired(false)
            .HasColumnName("discussion_id");
        
        builder.Property(v=> v.Status)
            .HasConversion<string>()
            .HasColumnName("status")
            .IsRequired();
        
        builder.Property(v=> v.RejectionComment)
            .HasConversion<string>()
            .HasColumnName("rejection_comment");
    }
}