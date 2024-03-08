using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Triplite_Committee_Platform.Migrations
{
    /// <inheritdoc />
    public partial class CreateBoard : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Board",
                columns: table => new
                {
                    BoardNo = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReqTypeID = table.Column<int>(type: "int", nullable: false),
                    National_ID = table.Column<int>(type: "int", nullable: false),
                    DeptNo = table.Column<int>(type: "int", nullable: false),
                    ReqStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReqDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ScholarshipModelNational_ID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Board", x => x.BoardNo);
                    table.ForeignKey(
                        name: "FK_Board_Department_DeptNo",
                        column: x => x.DeptNo,
                        principalTable: "Department",
                        principalColumn: "DeptNo",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Board_RequestType_ReqTypeID",
                        column: x => x.ReqTypeID,
                        principalTable: "RequestType",
                        principalColumn: "RequestTypeID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Board_Scholarship_National_ID",
                        column: x => x.National_ID,
                        principalTable: "Scholarship",
                        principalColumn: "National_ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Board_Scholarship_ScholarshipModelNational_ID",
                        column: x => x.ScholarshipModelNational_ID,
                        principalTable: "Scholarship",
                        principalColumn: "National_ID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Board_DeptNo",
                table: "Board",
                column: "DeptNo");

            migrationBuilder.CreateIndex(
                name: "IX_Board_National_ID",
                table: "Board",
                column: "National_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Board_ReqTypeID",
                table: "Board",
                column: "ReqTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_Board_ScholarshipModelNational_ID",
                table: "Board",
                column: "ScholarshipModelNational_ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Board");
        }
    }
}
