using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GamingNProgramming.DAL.Migrations
{
    /// <inheritdoc />
    public partial class addNumberToLevelAndAssignment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Number",
                table: "Map",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Number",
                table: "Level",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Number",
                table: "Assignment",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Number",
                table: "Map");

            migrationBuilder.DropColumn(
                name: "Number",
                table: "Level");

            migrationBuilder.DropColumn(
                name: "Number",
                table: "Assignment");
        }
    }
}
