using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace P2Project.Discussions.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Discussions_init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "discussions");

            migrationBuilder.CreateTable(
                name: "discussions",
                schema: "discussions",
                columns: table => new
                {
                    discussion_id = table.Column<Guid>(type: "uuid", nullable: false),
                    status = table.Column<string>(type: "text", nullable: false),
                    applicant_user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    reviewing_user_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_discussions", x => x.discussion_id);
                });

            migrationBuilder.CreateTable(
                name: "messages",
                schema: "discussions",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    message_content = table.Column<string>(type: "text", nullable: false),
                    sender_id = table.Column<Guid>(type: "uuid", nullable: false),
                    discussion_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    is_edited = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_messages", x => x.id);
                    table.ForeignKey(
                        name: "fk_messages_discussions_discussion_id",
                        column: x => x.discussion_id,
                        principalSchema: "discussions",
                        principalTable: "discussions",
                        principalColumn: "discussion_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_messages_discussion_id",
                schema: "discussions",
                table: "messages",
                column: "discussion_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "messages",
                schema: "discussions");

            migrationBuilder.DropTable(
                name: "discussions",
                schema: "discussions");
        }
    }
}
