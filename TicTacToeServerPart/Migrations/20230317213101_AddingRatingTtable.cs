using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TicTacToeServerPart.Migrations
{
    /// <inheritdoc />
    public partial class AddingRatingTtable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "registrationDate",
                table: "Players",
                newName: "RegistrationDate");

            migrationBuilder.AddColumn<int>(
                name: "Scores",
                table: "Players",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Scores",
                table: "Players");

            migrationBuilder.RenameColumn(
                name: "RegistrationDate",
                table: "Players",
                newName: "registrationDate");
        }
    }
}
