using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace P2Project.VolunteerRequests.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangeVolunteerRequestId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "request_Id",
                schema: "volunteer_requests",
                table: "volunteer_requests",
                newName: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "id",
                schema: "volunteer_requests",
                table: "volunteer_requests",
                newName: "request_Id");
        }
    }
}
