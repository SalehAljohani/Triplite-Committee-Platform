using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Triplite_Committee_Platform.Migrations
{
    /// <inheritdoc />
    public partial class JointTable_BoardSigns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "DeanSign",
                table: "Board",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "DeptMemeberSign1",
                table: "Board",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "DeptMemeberSign2",
                table: "Board",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "HeadofDeptSign",
                table: "Board",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ViceDeanSign",
                table: "Board",
                type: "bit",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "BoardSignatures",
                columns: table => new
                {
                    BoardNo = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BoardSignatures", x => new { x.BoardNo, x.UserId });
                    table.ForeignKey(
                        name: "FK_BoardSignatures_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BoardSignatures_Board_BoardNo",
                        column: x => x.BoardNo,
                        principalTable: "Board",
                        principalColumn: "BoardNo",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BoardSignatures_UserId",
                table: "BoardSignatures",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BoardSignatures");

            migrationBuilder.DropColumn(
                name: "DeanSign",
                table: "Board");

            migrationBuilder.DropColumn(
                name: "DeptMemeberSign1",
                table: "Board");

            migrationBuilder.DropColumn(
                name: "DeptMemeberSign2",
                table: "Board");

            migrationBuilder.DropColumn(
                name: "HeadofDeptSign",
                table: "Board");

            migrationBuilder.DropColumn(
                name: "ViceDeanSign",
                table: "Board");
        }
    }
}
