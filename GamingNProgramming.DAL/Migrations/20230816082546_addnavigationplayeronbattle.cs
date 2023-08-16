using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GamingNProgramming.DAL.Migrations
{
    /// <inheritdoc />
    public partial class addnavigationplayeronbattle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Battles_Player1Id",
                table: "Battles",
                column: "Player1Id");

            migrationBuilder.CreateIndex(
                name: "IX_Battles_Player2Id",
                table: "Battles",
                column: "Player2Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Battles_Players_Player1Id",
                table: "Battles",
                column: "Player1Id",
                principalTable: "Players",
                principalColumn: "UserId",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Battles_Players_Player2Id",
                table: "Battles",
                column: "Player2Id",
                principalTable: "Players",
                principalColumn: "UserId",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Battles_Players_Player1Id",
                table: "Battles");

            migrationBuilder.DropForeignKey(
                name: "FK_Battles_Players_Player2Id",
                table: "Battles");

            migrationBuilder.DropIndex(
                name: "IX_Battles_Player1Id",
                table: "Battles");

            migrationBuilder.DropIndex(
                name: "IX_Battles_Player2Id",
                table: "Battles");
        }
    }
}
