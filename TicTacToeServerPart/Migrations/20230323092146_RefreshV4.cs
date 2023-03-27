using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TicTacToeServerPart.Migrations
{
    /// <inheritdoc />
    public partial class RefreshV4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Players_LoginModelId",
                table: "Players");

            migrationBuilder.CreateIndex(
                name: "IX_Players_LoginModelId",
                table: "Players",
                column: "LoginModelId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Players_LoginModelId",
                table: "Players");

            migrationBuilder.CreateIndex(
                name: "IX_Players_LoginModelId",
                table: "Players",
                column: "LoginModelId");
        }
    }
}
