using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Triplite_Committee_Platform.Migrations
{
    /// <inheritdoc />
    public partial class ConnectedUser_Roles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RolesModelsUserModel",
                columns: table => new
                {
                    RolesRoleID = table.Column<int>(type: "int", nullable: false),
                    UserEmployeeID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolesModelsUserModel", x => new { x.RolesRoleID, x.UserEmployeeID });
                    table.ForeignKey(
                        name: "FK_RolesModelsUserModel_Roles_RolesRoleID",
                        column: x => x.RolesRoleID,
                        principalTable: "Roles",
                        principalColumn: "RoleID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RolesModelsUserModel_User_UserEmployeeID",
                        column: x => x.UserEmployeeID,
                        principalTable: "User",
                        principalColumn: "EmployeeID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RolesModelsUserModel_UserEmployeeID",
                table: "RolesModelsUserModel",
                column: "UserEmployeeID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RolesModelsUserModel");
        }
    }
}
