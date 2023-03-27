using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TicTacToeServerPart.Migrations
{
    /// <inheritdoc />
    public partial class RefrashV2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "EmailAddress",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "Password",
                table: "Players");

            migrationBuilder.AddColumn<int>(
                name: "LoginModelId",
                table: "Players",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Login",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmailAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Login", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Players_LoginModelId",
                table: "Players",
                column: "LoginModelId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Players_Login_LoginModelId",
                table: "Players",
                column: "LoginModelId",
                principalTable: "Login",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Players_Login_LoginModelId",
                table: "Players");

            migrationBuilder.DropTable(
                name: "Login");

            migrationBuilder.DropIndex(
                name: "IX_Players_LoginModelId",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "LoginModelId",
                table: "Players");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Players",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "EmailAddress",
                table: "Players",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "Players",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
