//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tarteeb.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddMilestoneIdToTicketTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "MilestoneId",
                table: "Tickets",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_MilestoneId",
                table: "Tickets",
                column: "MilestoneId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_Milestones_MilestoneId",
                table: "Tickets",
                column: "MilestoneId",
                principalTable: "Milestones",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_Milestones_MilestoneId",
                table: "Tickets");

            migrationBuilder.DropIndex(
                name: "IX_Tickets_MilestoneId",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "MilestoneId",
                table: "Tickets");
        }
    }
}
