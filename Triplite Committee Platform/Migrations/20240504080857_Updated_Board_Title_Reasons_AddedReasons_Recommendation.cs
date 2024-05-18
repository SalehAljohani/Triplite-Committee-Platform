using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Triplite_Committee_Platform.Migrations
{
    /// <inheritdoc />
    public partial class Updated_Board_Title_Reasons_AddedReasons_Recommendation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AddedReasons",
                table: "Board",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Reasons",
                table: "Board",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "Recommendation",
                table: "Board",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Board",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AddedReasons",
                table: "Board");

            migrationBuilder.DropColumn(
                name: "Reasons",
                table: "Board");

            migrationBuilder.DropColumn(
                name: "Recommendation",
                table: "Board");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Board");
        }
    }
}
