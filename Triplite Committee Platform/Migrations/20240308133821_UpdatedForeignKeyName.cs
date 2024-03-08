using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Triplite_Committee_Platform.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedForeignKeyName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Scholarship_Department_DepartmentDeptNo",
                table: "Scholarship");

            migrationBuilder.DropIndex(
                name: "IX_Scholarship_DepartmentDeptNo",
                table: "Scholarship");

            migrationBuilder.DropColumn(
                name: "DepartmentDeptNo",
                table: "Scholarship");

            migrationBuilder.CreateIndex(
                name: "IX_Scholarship_DeptNo",
                table: "Scholarship",
                column: "DeptNo");

            migrationBuilder.AddForeignKey(
                name: "FK_Scholarship_Department_DeptNo",
                table: "Scholarship",
                column: "DeptNo",
                principalTable: "Department",
                principalColumn: "DeptNo",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Scholarship_Department_DeptNo",
                table: "Scholarship");

            migrationBuilder.DropIndex(
                name: "IX_Scholarship_DeptNo",
                table: "Scholarship");

            migrationBuilder.AddColumn<int>(
                name: "DepartmentDeptNo",
                table: "Scholarship",
                type: "int",
                nullable: false,
                defaultValue: 0);

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
    }
}
