using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Triplite_Committee_Platform.Migrations
{
    /// <inheritdoc />
    public partial class CreateFile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "File",
                columns: table => new
                {
                    FileID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BoardNo = table.Column<int>(type: "int", nullable: false),
                    Creation_Date = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_File", x => x.FileID);
                    table.ForeignKey(
                        name: "FK_File_Board_BoardNo",
                        column: x => x.BoardNo,
                        principalTable: "Board",
                        principalColumn: "BoardNo",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_File_BoardNo",
                table: "File",
                column: "BoardNo");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "File");
        }
    }
}
