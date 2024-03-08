using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Triplite_Committee_Platform.Migrations
{
    /// <inheritdoc />
    public partial class CreateReasons : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ReasonsModel",
                columns: table => new
                {
                    ReasonID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReqTypeID = table.Column<int>(type: "int", nullable: false),
                    Context = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Connected = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReasonsModel", x => x.ReasonID);
                    table.ForeignKey(
                        name: "FK_ReasonsModel_RequestType_ReqTypeID",
                        column: x => x.ReqTypeID,
                        principalTable: "RequestType",
                        principalColumn: "RequestTypeID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReasonsModel_ReqTypeID",
                table: "ReasonsModel",
                column: "ReqTypeID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReasonsModel");
        }
    }
}
