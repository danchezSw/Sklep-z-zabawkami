using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ToyStore.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddAdminReplyToComments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AdminReply",
                table: "Comments",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ReplyDate",
                table: "Comments",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdminReply",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "ReplyDate",
                table: "Comments");
        }
    }
}
