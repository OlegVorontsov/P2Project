using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace P2Project.VolunteerRequests.Infrastructure.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveOutbox : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "idx_outbox_messages_unprocessed",
                schema: "volunteer_requests",
                table: "outbox_messages");

            migrationBuilder.AlterColumn<string>(
                name: "type",
                schema: "volunteer_requests",
                table: "outbox_messages",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(2000)",
                oldMaxLength: 2000);

            migrationBuilder.AlterColumn<string>(
                name: "payload",
                schema: "volunteer_requests",
                table: "outbox_messages",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "jsonb",
                oldMaxLength: 2000);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "type",
                schema: "volunteer_requests",
                table: "outbox_messages",
                type: "character varying(2000)",
                maxLength: 2000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "payload",
                schema: "volunteer_requests",
                table: "outbox_messages",
                type: "jsonb",
                maxLength: 2000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.CreateIndex(
                name: "idx_outbox_messages_unprocessed",
                schema: "volunteer_requests",
                table: "outbox_messages",
                columns: new[] { "occurred_on_utc", "processed_on_utc" },
                filter: "processed_on_utc IS NULL")
                .Annotation("Npgsql:IndexInclude", new[] { "id", "type", "payload" });
        }
    }
}
