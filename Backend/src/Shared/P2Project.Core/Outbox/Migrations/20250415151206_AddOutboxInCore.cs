using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace P2Project.Core.Outbox.Migrations
{
    /// <inheritdoc />
    public partial class AddOutboxInCore : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "outbox");

            migrationBuilder.CreateTable(
                name: "outbox_messages",
                schema: "outbox",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    type = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    payload = table.Column<string>(type: "jsonb", maxLength: 2000, nullable: false),
                    occurred_on_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    processed_on_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    error = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_outbox_messages", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "idx_outbox_messages_unprocessed",
                schema: "outbox",
                table: "outbox_messages",
                columns: new[] { "occurred_on_utc", "processed_on_utc" },
                filter: "processed_on_utc IS NULL")
                .Annotation("Npgsql:IndexInclude", new[] { "id", "type", "payload" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "outbox_messages",
                schema: "outbox");
        }
    }
}
