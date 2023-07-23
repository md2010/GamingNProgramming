using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GamingNProgramming.DAL.Migrations
{
    /// <inheritdoc />
    public partial class addplayertaskanswertable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.CreateTable(
                name: "PlayersTasksAnswers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DateUpdated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PlayerTaskId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AnswerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayersTasksAnswers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlayersTasksAnswers_Answers_AnswerId",
                        column: x => x.AnswerId,
                        principalTable: "Answers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlayersTasksAnswers_PlayersTasks_PlayerTaskId",
                        column: x => x.PlayerTaskId,
                        principalTable: "PlayersTasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
          
            migrationBuilder.CreateIndex(
                name: "IX_PlayersTasksAnswers_AnswerId",
                table: "PlayersTasksAnswers",
                column: "AnswerId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayersTasksAnswers_PlayerTaskId",
                table: "PlayersTasksAnswers",
                column: "PlayerTaskId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
           
            migrationBuilder.DropTable(
                name: "PlayersTasksAnswers");
                       
        }
    }
}
