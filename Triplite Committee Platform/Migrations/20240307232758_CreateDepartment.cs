using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Triplite_Committee_Platform.Migrations
{
    /// <inheritdoc />
    public partial class CreateDepartment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DepartmentModel_College_CollegeModelCollegeNo",
                table: "DepartmentModel");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DepartmentModel",
                table: "DepartmentModel");

            migrationBuilder.RenameTable(
                name: "DepartmentModel",
                newName: "Department");

            migrationBuilder.RenameIndex(
                name: "IX_DepartmentModel_CollegeModelCollegeNo",
                table: "Department",
                newName: "IX_Department_CollegeModelCollegeNo");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Department",
                table: "Department",
                column: "DeptNo");

            migrationBuilder.AddForeignKey(
                name: "FK_Department_College_CollegeModelCollegeNo",
                table: "Department",
                column: "CollegeModelCollegeNo",
                principalTable: "College",
                principalColumn: "CollegeNo");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Department_College_CollegeModelCollegeNo",
                table: "Department");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Department",
                table: "Department");

            migrationBuilder.RenameTable(
                name: "Department",
                newName: "DepartmentModel");

            migrationBuilder.RenameIndex(
                name: "IX_Department_CollegeModelCollegeNo",
                table: "DepartmentModel",
                newName: "IX_DepartmentModel_CollegeModelCollegeNo");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DepartmentModel",
                table: "DepartmentModel",
                column: "DeptNo");

            migrationBuilder.AddForeignKey(
                name: "FK_DepartmentModel_College_CollegeModelCollegeNo",
                table: "DepartmentModel",
                column: "CollegeModelCollegeNo",
                principalTable: "College",
                principalColumn: "CollegeNo");
        }
    }
}
