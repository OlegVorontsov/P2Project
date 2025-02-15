using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace P2Project.Accounts.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Accounts_AddUntliBan : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "banned_for_requests_until",
                schema: "accounts",
                table: "participant_accounts",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "banned_for_requests_until",
                schema: "accounts",
                table: "participant_accounts");
        }
    }
}
