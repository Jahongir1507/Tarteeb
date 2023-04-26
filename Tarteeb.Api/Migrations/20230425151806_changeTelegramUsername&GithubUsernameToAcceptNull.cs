using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tarteeb.Api.Migrations
{
    /// <inheritdoc />
    public partial class changeTelegramUsernameGithubUsernameToAcceptNull : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_GitHubUsername",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_TelegramUsername",
                table: "Users");

            migrationBuilder.CreateIndex(
                name: "IX_Users_GitHubUsername",
                table: "Users",
                column: "GitHubUsername",
                unique: true,
                filter: "[GitHubUsername] NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Users_TelegramUsername",
                table: "Users",
                column: "TelegramUsername",
                unique: true,
                filter: "[TelegramUsername] NULL");
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
    }
}
