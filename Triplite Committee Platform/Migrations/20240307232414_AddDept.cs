using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Triplite_Committee_Platform.Migrations
{
    /// <inheritdoc />
    public partial class AddDept : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DepartmentModel",
                columns: table => new
                {
                    DeptNo = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DeptName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CollegeModelCollegeNo = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DepartmentModel", x => x.DeptNo);
                    table.ForeignKey(
                        name: "FK_DepartmentModel_College_CollegeModelCollegeNo",
                        column: x => x.CollegeModelCollegeNo,
                        principalTable: "College",
                        principalColumn: "CollegeNo");
                });

            migrationBuilder.CreateIndex(
                name: "IX_DepartmentModel_CollegeModelCollegeNo",
                table: "DepartmentModel",
                column: "CollegeModelCollegeNo");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DepartmentModel");
        }
    }
}
