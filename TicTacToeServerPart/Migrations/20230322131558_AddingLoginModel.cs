using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TicTacToeServerPart.Migrations
{
    /// <inheritdoc />
    public partial class AddingLoginModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Players",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "Players",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "Password",
                table: "Players");
        }
    }
}
