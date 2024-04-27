using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Triplite_Committee_Platform.Migrations
{
    /// <inheritdoc />
    public partial class Update_Scholarship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Board_Scholarship_National_ID",
                table: "Board");

            migrationBuilder.DropForeignKey(
                name: "FK_Board_Scholarship_ScholarshipModelNational_ID",
                table: "Board");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Scholarship",
                table: "Scholarship");

            migrationBuilder.DropIndex(
                name: "IX_Board_ScholarshipModelNational_ID",
                table: "Board");

            migrationBuilder.DropColumn(
                name: "ScholarshipModelNational_ID",
                table: "Board");

            migrationBuilder.DropColumn(
                name: "National_ID",
                table: "Scholarship");

            migrationBuilder.AddColumn<string>(
                name: "National_ID",
                table: "Scholarship",
                type: "nvarchar(max)",
                nullable: false);

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Scholarship",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Scholarship",
                table: "Scholarship",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Board_Scholarship_National_ID",
                table: "Board",
                column: "National_ID",
                principalTable: "Scholarship",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Board_Scholarship_National_ID",
                table: "Board");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Scholarship",
                table: "Scholarship");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Scholarship");

            migrationBuilder.DropColumn(
                name: "National_ID",
                table: "Scholarship");

            migrationBuilder.AddColumn<int>(
                name: "National_ID",
                table: "Scholarship",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "ScholarshipModelNational_ID",
                table: "Board",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Scholarship",
                table: "Scholarship",
                column: "National_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Board_ScholarshipModelNational_ID",
                table: "Board",
                column: "ScholarshipModelNational_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Board_Scholarship_National_ID",
                table: "Board",
                column: "National_ID",
                principalTable: "Scholarship",
                principalColumn: "National_ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Board_Scholarship_ScholarshipModelNational_ID",
                table: "Board",
                column: "ScholarshipModelNational_ID",
                principalTable: "Scholarship",
                principalColumn: "National_ID");
        }
    }
}
