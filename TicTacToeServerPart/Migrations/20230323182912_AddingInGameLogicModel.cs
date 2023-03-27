using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TicTacToeServerPart.Migrations
{
    /// <inheritdoc />
    public partial class AddingInGameLogicModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Players_LoginModelId",
                table: "Players");

            migrationBuilder.CreateTable(
                name: "InGameLogic",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WinnerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InGameLogic", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Players_LoginModelId",
                table: "Players",
                column: "LoginModelId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InGameLogic");

            migrationBuilder.DropIndex(
                name: "IX_Players_LoginModelId",
                table: "Players");

            migrationBuilder.CreateIndex(
                name: "IX_Players_LoginModelId",
                table: "Players",
                column: "LoginModelId",
                unique: true);
        }
    }
}
