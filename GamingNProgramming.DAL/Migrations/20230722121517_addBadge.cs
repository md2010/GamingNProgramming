using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GamingNProgramming.DAL.Migrations
{
    /// <inheritdoc />
    public partial class addBadge : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assignment_Level_LevelId",
                table: "Assignment");

            migrationBuilder.RenameColumn(
                name: "NumberOfLevels",
                table: "Map",
                newName: "Points");

            migrationBuilder.RenameColumn(
                name: "NumberOfTasks",
                table: "Level",
                newName: "Points");

            migrationBuilder.AddColumn<bool>(
                name: "IsVisible",
                table: "Map",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<Guid>(
                name: "LevelId",
                table: "Assignment",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "BadgeId",
                table: "Assignment",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Badge",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DateUpdated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Path = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Badge", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Assignment_BadgeId",
                table: "Assignment",
                column: "BadgeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Assignment_Badge_BadgeId",
                table: "Assignment",
                column: "BadgeId",
                principalTable: "Badge",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Assignment_Level_LevelId",
                table: "Assignment",
                column: "LevelId",
                principalTable: "Level",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assignment_Badge_BadgeId",
                table: "Assignment");

            migrationBuilder.DropForeignKey(
                name: "FK_Assignment_Level_LevelId",
                table: "Assignment");

            migrationBuilder.DropTable(
                name: "Badge");

            migrationBuilder.DropIndex(
                name: "IX_Assignment_BadgeId",
                table: "Assignment");

            migrationBuilder.DropColumn(
                name: "IsVisible",
                table: "Map");

            migrationBuilder.DropColumn(
                name: "BadgeId",
                table: "Assignment");

            migrationBuilder.RenameColumn(
                name: "Points",
                table: "Map",
                newName: "NumberOfLevels");

            migrationBuilder.RenameColumn(
                name: "Points",
                table: "Level",
                newName: "NumberOfTasks");

            migrationBuilder.AlterColumn<Guid>(
                name: "LevelId",
                table: "Assignment",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_Assignment_Level_LevelId",
                table: "Assignment",
                column: "LevelId",
                principalTable: "Level",
                principalColumn: "Id");
        }
    }
}
