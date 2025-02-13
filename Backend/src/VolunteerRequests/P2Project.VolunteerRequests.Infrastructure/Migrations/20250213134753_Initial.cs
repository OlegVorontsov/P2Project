using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace P2Project.VolunteerRequests.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "volunteer_requests");

            migrationBuilder.CreateTable(
                name: "volunteer_requests",
                schema: "volunteer_requests",
                columns: table => new
                {
                    request_Id = table.Column<Guid>(type: "uuid", nullable: false),
                    admin_Id = table.Column<Guid>(type: "uuid", nullable: true),
                    user_Id = table.Column<Guid>(type: "uuid", nullable: false),
                    discussion_id = table.Column<Guid>(type: "uuid", nullable: true),
                    status = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    rejection_comment = table.Column<string>(type: "text", nullable: true),
                    first_name = table.Column<string>(type: "text", nullable: false),
                    last_name = table.Column<string>(type: "text", nullable: false),
                    second_name = table.Column<string>(type: "text", nullable: false),
                    age = table.Column<int>(type: "integer", nullable: false),
                    grade = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_volunteer_requests", x => x.request_Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "volunteer_requests",
                schema: "volunteer_requests");
        }
    }
}
