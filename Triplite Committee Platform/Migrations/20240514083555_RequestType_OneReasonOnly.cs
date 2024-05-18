using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Triplite_Committee_Platform.Migrations
{
    /// <inheritdoc />
    public partial class RequestType_OneReasonOnly : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Reasons_ReqTypeID",
                table: "Reasons");

            migrationBuilder.CreateIndex(
                name: "IX_Reasons_ReqTypeID",
                table: "Reasons",
                column: "ReqTypeID",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Reasons_ReqTypeID",
                table: "Reasons");

            migrationBuilder.CreateIndex(
                name: "IX_Reasons_ReqTypeID",
                table: "Reasons",
                column: "ReqTypeID");
        }
    }
}
