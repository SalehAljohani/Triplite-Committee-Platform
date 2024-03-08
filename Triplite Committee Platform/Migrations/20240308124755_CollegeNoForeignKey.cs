using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Triplite_Committee_Platform.Migrations
{
    /// <inheritdoc />
    public partial class CollegeNoForeignKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Department_College_CollegeModelCollegeNo",
                table: "Department");

            migrationBuilder.DropIndex(
                name: "IX_Department_CollegeModelCollegeNo",
                table: "Department");

            migrationBuilder.DropColumn(
                name: "CollegeModelCollegeNo",
                table: "Department");

            migrationBuilder.AddColumn<int>(
                name: "CollegeNo",
                table: "Department",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CollegeNo",
                table: "Department");

            migrationBuilder.AddColumn<int>(
                name: "CollegeModelCollegeNo",
                table: "Department",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Department_CollegeModelCollegeNo",
                table: "Department",
                column: "CollegeModelCollegeNo");

            migrationBuilder.AddForeignKey(
                name: "FK_Department_College_CollegeModelCollegeNo",
                table: "Department",
                column: "CollegeModelCollegeNo",
                principalTable: "College",
                principalColumn: "CollegeNo");
        }
    }
}
