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
            .IsRequired();

        builder.Property(n => n.Email)
            .IsRequired(false);

        builder.OwnsOne(n => n.TelegramSettings, ts =>
        {
            ts.ToJson("telegram_settings");
            
            ts.Property(s => s.UserId)
                .IsRequired();

            ts.Property(s => s.ChatId)
                .IsRequired();
        });

        builder.Property(n => n.IsWebSend)
            .IsRequired(false);
    }
}