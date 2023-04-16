//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tarteeb.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddedUserFieldToMilestone : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Milestones_AssigneeId",
                table: "Milestones",
                column: "AssigneeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Milestones_Users_AssigneeId",
                table: "Milestones",
                column: "AssigneeId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Milestones_Users_AssigneeId",
                table: "Milestones");

            migrationBuilder.DropIndex(
                name: "IX_Milestones_AssigneeId",
                table: "Milestones");
        }
    }
}
