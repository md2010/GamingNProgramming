using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GamingNProgramming.DAL.Migrations
{
    /// <inheritdoc />
    public partial class addbadgetoplayertask : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "BadgeId",
                table: "PlayersTasks",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PlayersTasks_BadgeId",
                table: "PlayersTasks",
                column: "BadgeId");

            migrationBuilder.AddForeignKey(
                name: "FK_PlayersTasks_Badges_BadgeId",
                table: "PlayersTasks",
                column: "BadgeId",
                principalTable: "Badges",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlayersTasks_Badges_BadgeId",
                table: "PlayersTasks");

            migrationBuilder.DropIndex(
                name: "IX_PlayersTasks_BadgeId",
                table: "PlayersTasks");

            migrationBuilder.DropColumn(
                name: "BadgeId",
                table: "PlayersTasks");
        }
    }
}
