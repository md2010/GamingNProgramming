using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GamingNProgramming.DAL.Migrations
{
    /// <inheritdoc />
    public partial class addWonId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "WonId",
                table: "Battles",
                type: "uniqueidentifier",
                nullable: true);

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
           
            migrationBuilder.DropColumn(
                name: "WonId",
                table: "Battles");

            
        }
    }
}
