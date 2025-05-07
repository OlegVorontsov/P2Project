using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace P2Project.VolunteerRequests.Infrastructure.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddGenderToVolunteerRequest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "outbox_messages",
                schema: "volunteer_requests");

            migrationBuilder.AddColumn<string>(
                name: "gender",
                schema: "volunteer_requests",
                table: "volunteer_requests",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "gender",
                schema: "volunteer_requests",
                table: "volunteer_requests");

            migrationBuilder.CreateTable(
                name: "outbox_messages",
                schema: "volunteer_requests",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    error = table.Column<string>(type: "text", nullable: true),
                    occurred_on_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    payload = table.Column<string>(type: "text", nullable: false),
                    processed_on_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    type = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_outbox_messages", x => x.id);
                });
        }
    }
}
