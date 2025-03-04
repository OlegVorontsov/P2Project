using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace P2Project.Volunteers.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveMyArrayInVolunteer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "my_array",
                schema: "volunteers",
                table: "volunteers");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int[]>(
                name: "my_array",
                schema: "volunteers",
                table: "volunteers",
                type: "integer[]",
                nullable: false,
                defaultValue: new int[0]);
        }
    }
}
