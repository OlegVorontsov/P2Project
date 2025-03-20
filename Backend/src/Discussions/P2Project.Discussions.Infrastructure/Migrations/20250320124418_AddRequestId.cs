using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace P2Project.Discussions.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddRequestId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_messages_discussions_discussion_id",
                schema: "discussions",
                table: "messages");

            migrationBuilder.DropPrimaryKey(
                name: "pk_discussions",
                schema: "discussions",
                table: "discussions");

            migrationBuilder.RenameColumn(
                name: "discussion_id",
                schema: "discussions",
                table: "discussions",
                newName: "request_id");

            migrationBuilder.AddColumn<Guid>(
                name: "id",
                schema: "discussions",
                table: "discussions",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "pk_discussions",
                schema: "discussions",
                table: "discussions",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_messages_discussions_discussion_id",
                schema: "discussions",
                table: "messages",
                column: "discussion_id",
                principalSchema: "discussions",
                principalTable: "discussions",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_messages_discussions_discussion_id",
                schema: "discussions",
                table: "messages");

            migrationBuilder.DropPrimaryKey(
                name: "pk_discussions",
                schema: "discussions",
                table: "discussions");

            migrationBuilder.DropColumn(
                name: "id",
                schema: "discussions",
                table: "discussions");

            migrationBuilder.RenameColumn(
                name: "request_id",
                schema: "discussions",
                table: "discussions",
                newName: "discussion_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_discussions",
                schema: "discussions",
                table: "discussions",
                column: "discussion_id");

            migrationBuilder.AddForeignKey(
                name: "fk_messages_discussions_discussion_id",
                schema: "discussions",
                table: "messages",
                column: "discussion_id",
                principalSchema: "discussions",
                principalTable: "discussions",
                principalColumn: "discussion_id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
