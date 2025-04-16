using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NotificationService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddEmail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "is_email_send",
                schema: "notifications",
                table: "UserNotificationSettings");

            migrationBuilder.AddColumn<string>(
                name: "email",
                schema: "notifications",
                table: "UserNotificationSettings",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "email",
                schema: "notifications",
                table: "UserNotificationSettings");

            migrationBuilder.AddColumn<bool>(
                name: "is_email_send",
                schema: "notifications",
                table: "UserNotificationSettings",
                type: "boolean",
                nullable: true);
        }
    }
}
