using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GamingNProgramming.DAL.Migrations
{
    /// <inheritdoc />
    public partial class addplayertasktable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {                       
            migrationBuilder.RenameIndex(
                name: "IX_TestCase_AssignmentId",
                table: "TestCases",
                newName: "IX_TestCases_AssignmentId");

            migrationBuilder.RenameIndex(
                name: "IX_Map_ProfessorId",
                table: "Maps",
                newName: "IX_Maps_ProfessorId");

            migrationBuilder.RenameIndex(
                name: "IX_Level_MapId",
                table: "Levels",
                newName: "IX_Levels_MapId");
           
            migrationBuilder.RenameIndex(
                name: "IX_Assignment_BadgeId",
                table: "Assignments",
                newName: "IX_Assignments_BadgeId");

            migrationBuilder.RenameIndex(
                name: "IX_Answer_AssignmentId",
                table: "Answers",
                newName: "IX_Answers_AssignmentId");                      

            migrationBuilder.CreateTable(
                name: "PlayersTasks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DateUpdated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PlayerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AssignmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ScoredPoints = table.Column<double>(type: "float", nullable: false),
                    Percentage = table.Column<double>(type: "float", nullable: false),
                    PlayersCode = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayersTasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlayersTasks_Assignments_AssignmentId",
                        column: x => x.AssignmentId,
                        principalTable: "Assignments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlayersTasks_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });
         
            migrationBuilder.CreateIndex(
                name: "IX_PlayersTasks_AssignmentId",
                table: "PlayersTasks",
                column: "AssignmentId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayersTasks_PlayerId",
                table: "PlayersTasks",
                column: "PlayerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Answers_Assignments_AssignmentId",
                table: "Answers",
                column: "AssignmentId",
                principalTable: "Assignments",
                principalColumn: "Id");
           
            migrationBuilder.AddForeignKey(
                name: "FK_Assignments_Badges_BadgeId",
                table: "Assignments",
                column: "BadgeId",
                principalTable: "Badges",
                principalColumn: "Id");
           
            migrationBuilder.AddForeignKey(
                name: "FK_Levels_Maps_MapId",
                table: "Levels",
                column: "MapId",
                principalTable: "Maps",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Maps_Professors_ProfessorId",
                table: "Maps",
                column: "ProfessorId",
                principalTable: "Professors",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TestCases_Assignments_AssignmentId",
                table: "TestCases",
                column: "AssignmentId",
                principalTable: "Assignments",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {           
            migrationBuilder.DropTable(
                name: "PlayersTasks");

           
            migrationBuilder.RenameTable(
                name: "TestCases",
                newName: "TestCase");

            migrationBuilder.RenameTable(
                name: "Maps",
                newName: "Map");

            migrationBuilder.RenameTable(
                name: "Levels",
                newName: "Level");

            migrationBuilder.RenameTable(
                name: "Badges",
                newName: "Badge");

            migrationBuilder.RenameTable(
                name: "Assignments",
                newName: "Assignment");

            migrationBuilder.RenameTable(
                name: "Answers",
                newName: "Answer");

            migrationBuilder.RenameIndex(
                name: "IX_TestCases_AssignmentId",
                table: "TestCase",
                newName: "IX_TestCase_AssignmentId");

            migrationBuilder.RenameIndex(
                name: "IX_Maps_ProfessorId",
                table: "Map",
                newName: "IX_Map_ProfessorId");

            migrationBuilder.RenameIndex(
                name: "IX_Levels_MapId",
                table: "Level",
                newName: "IX_Level_MapId");
      
            migrationBuilder.RenameIndex(
                name: "IX_Assignments_BadgeId",
                table: "Assignment",
                newName: "IX_Assignment_BadgeId");

            migrationBuilder.RenameIndex(
                name: "IX_Answers_AssignmentId",
                table: "Answer",
                newName: "IX_Answer_AssignmentId");           
        }
    }
}
