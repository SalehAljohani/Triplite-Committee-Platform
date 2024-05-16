using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Triplite_Committee_Platform.Migrations
{
    /// <inheritdoc />
    public partial class Board_SingedUsersList : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BoardModelBoardNo",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_BoardModelBoardNo",
                table: "AspNetUsers",
                column: "BoardModelBoardNo");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Board_BoardModelBoardNo",
                table: "AspNetUsers",
                column: "BoardModelBoardNo",
                principalTable: "Board",
                principalColumn: "BoardNo");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Board_BoardModelBoardNo",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_BoardModelBoardNo",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "BoardModelBoardNo",
                table: "AspNetUsers");
        }
    }
}
