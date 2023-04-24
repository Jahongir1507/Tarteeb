//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tarteeb.Api.Migrations
{
    /// <inheritdoc />
    public partial class MadeUniqueGithubTelegramUsername : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "TelegramUsername",
                table: "Users",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "GitHubUsername",
                table: "Users",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_GitHubUsername",
                table: "Users",
                column: "GitHubUsername",
                unique: true,
                filter: "[GitHubUsername] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Users_TelegramUsername",
                table: "Users",
                column: "TelegramUsername",
                unique: true,
                filter: "[TelegramUsername] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_GitHubUsername",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_TelegramUsername",
                table: "Users");

            migrationBuilder.AlterColumn<string>(
                name: "TelegramUsername",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "GitHubUsername",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);
        }
    }
}
