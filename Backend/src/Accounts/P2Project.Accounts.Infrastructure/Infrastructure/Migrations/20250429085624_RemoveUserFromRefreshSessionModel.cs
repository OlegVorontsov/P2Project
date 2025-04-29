using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace P2Project.Accounts.Infrastructure.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveUserFromRefreshSessionModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_refresh_sessions_user_user_id",
                schema: "accounts",
                table: "refresh_sessions");

            migrationBuilder.DropIndex(
                name: "ix_refresh_sessions_user_id",
                schema: "accounts",
                table: "refresh_sessions");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "ix_refresh_sessions_user_id",
                schema: "accounts",
                table: "refresh_sessions",
                column: "user_id");

            migrationBuilder.AddForeignKey(
                name: "fk_refresh_sessions_user_user_id",
                schema: "accounts",
                table: "refresh_sessions",
                column: "user_id",
                principalSchema: "accounts",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
