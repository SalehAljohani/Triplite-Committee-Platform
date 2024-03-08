using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Triplite_Committee_Platform.Migrations
{
    /// <inheritdoc />
    public partial class ForeignKeyCollege : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Department_CollegeNo",
                table: "Department",
                column: "CollegeNo");

            migrationBuilder.AddForeignKey(
                name: "FK_Department_College_CollegeNo",
                table: "Department",
                column: "CollegeNo",
                principalTable: "College",
                principalColumn: "CollegeNo",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Department_College_CollegeNo",
                table: "Department");

            migrationBuilder.DropIndex(
                name: "IX_Department_CollegeNo",
                table: "Department");
        }
    }
}
