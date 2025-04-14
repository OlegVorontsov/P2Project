using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NotificationService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddTelegramSettings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "is_telegram_send",
                schema: "notifications",
                table: "UserNotificationSettings");

            migrationBuilder.DropColumn(
                name: "telegram_chat_id",
                schema: "notifications",
                table: "UserNotificationSettings");

            migrationBuilder.AddColumn<string>(
                name: "telegram_settings",
                schema: "notifications",
                table: "UserNotificationSettings",
                type: "jsonb",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "telegram_settings",
                schema: "notifications",
                table: "UserNotificationSettings");

            migrationBuilder.AddColumn<bool>(
                name: "is_telegram_send",
                schema: "notifications",
                table: "UserNotificationSettings",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "telegram_chat_id",
                schema: "notifications",
                table: "UserNotificationSettings",
                type: "bigint",
                nullable: true);
        }
    }
}
