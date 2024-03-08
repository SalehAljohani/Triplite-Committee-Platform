using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Triplite_Committee_Platform.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedDeptNoFK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DeptNo",
                table: "Scholarship",
                newName: "DepartmentDeptNo");

            migrationBuilder.CreateIndex(
                name: "IX_Scholarship_DepartmentDeptNo",
                table: "Scholarship",
                column: "DepartmentDeptNo");

            migrationBuilder.AddForeignKey(
                name: "FK_Scholarship_Department_DepartmentDeptNo",
                table: "Scholarship",
                column: "DepartmentDeptNo",
                principalTable: "Department",
                principalColumn: "DeptNo",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Scholarship_Department_DepartmentDeptNo",
                table: "Scholarship");

            migrationBuilder.DropIndex(
                name: "IX_Scholarship_DepartmentDeptNo",
                table: "Scholarship");

            migrationBuilder.RenameColumn(
                name: "DepartmentDeptNo",
                table: "Scholarship",
                newName: "DeptNo");
        }
    }
}
