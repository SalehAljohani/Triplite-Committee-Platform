using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Triplite_Committee_Platform.Migrations
{
    /// <inheritdoc />
    public partial class Update_Board : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Board_Scholarship_National_ID",
                table: "Board");

            migrationBuilder.RenameColumn(
                name: "National_ID",
                table: "Board",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_Board_National_ID",
                table: "Board",
                newName: "IX_Board_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Board_Scholarship_Id",
                table: "Board",
                column: "Id",
                principalTable: "Scholarship",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Board_Scholarship_Id",
                table: "Board");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Board",
                newName: "National_ID");

            migrationBuilder.RenameIndex(
                name: "IX_Board_Id",
                table: "Board",
                newName: "IX_Board_National_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Board_Scholarship_National_ID",
                table: "Board",
                column: "National_ID",
                principalTable: "Scholarship",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
