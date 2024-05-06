using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Triplite_Committee_Platform.Migrations
{
    /// <inheritdoc />
    public partial class Carousel_Linked_toDepartments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DeptNo",
                table: "CarouselItem",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_CarouselItem_DeptNo",
                table: "CarouselItem",
                column: "DeptNo");

            migrationBuilder.AddForeignKey(
                name: "FK_CarouselItem_Department_DeptNo",
                table: "CarouselItem",
                column: "DeptNo",
                principalTable: "Department",
                principalColumn: "DeptNo",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CarouselItem_Department_DeptNo",
                table: "CarouselItem");

            migrationBuilder.DropIndex(
                name: "IX_CarouselItem_DeptNo",
                table: "CarouselItem");

            migrationBuilder.DropColumn(
                name: "DeptNo",
                table: "CarouselItem");
        }
    }
}
