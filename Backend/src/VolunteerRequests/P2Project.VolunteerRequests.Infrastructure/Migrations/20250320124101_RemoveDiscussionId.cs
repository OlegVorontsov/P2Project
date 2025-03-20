using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace P2Project.VolunteerRequests.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveDiscussionId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "discussion_id",
                schema: "volunteer_requests",
                table: "volunteer_requests");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "discussion_id",
                schema: "volunteer_requests",
                table: "volunteer_requests",
                type: "uuid",
                nullable: true);
        }
    }
}
