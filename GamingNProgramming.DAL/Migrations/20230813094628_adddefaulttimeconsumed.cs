using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GamingNProgramming.DAL.Migrations
{
    /// <inheritdoc />
    public partial class adddefaulttimeconsumed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DefultPoints",
                table: "Players",
                newName: "DefaultPoints");

            migrationBuilder.AddColumn<long>(
                name: "DefaultTimeConsumed",
                table: "Players",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DefaultTimeConsumed",
                table: "Players");

            migrationBuilder.RenameColumn(
                name: "DefaultPoints",
                table: "Players",
                newName: "DefultPoints");
        }
    }
}
