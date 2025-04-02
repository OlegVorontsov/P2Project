using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NotificationService.Domain;

namespace NotificationService.Infrastructure.Configurations.Write;

public class UserNotificationSettingsConfiguration :
    IEntityTypeConfiguration<UserNotificationSettings>
{
    public void Configure(EntityTypeBuilder<UserNotificationSettings> builder)
    {
        builder.ToTable("UserNotificationSettings");

        builder.HasKey(n => n.Id);
        
        builder.Property(n => n.UserId)
            .IsRequired()
            .HasColumnName("user_id");

        builder.Property(n => n.IsEmailSend)
            .IsRequired(false)
            .HasColumnName("is_email_send");

        builder.Property(n => n.IsTelegramSend)
            .IsRequired(false)
            .HasColumnName("is_telegram_send");

        builder.Property(n => n.IsWebSend)
            .IsRequired(false)
            .HasColumnName("is_web_send");
    }
}