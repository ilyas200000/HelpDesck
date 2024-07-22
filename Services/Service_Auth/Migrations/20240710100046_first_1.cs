using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Service_Auth.Migrations
{
    public partial class first_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ApplicationRoleId",
                table: "AspNetRoleClaims",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "DROITS",
                columns: table => new
                {
                    DRT_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DRT_LIB = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ApplicationRoleId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DROITS", x => x.DRT_ID);
                    table.ForeignKey(
                        name: "FK_DROITS_AspNetRoles_ApplicationRoleId",
                        column: x => x.ApplicationRoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "DROIT_ROLE",
                columns: table => new
                {
                    DROIT_ROLE_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DRT_ID = table.Column<int>(type: "int", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DROIT_ROLE", x => x.DROIT_ROLE_ID);
                    table.ForeignKey(
                        name: "FK_DROIT_ROLE_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DROIT_ROLE_DROITS_DRT_ID",
                        column: x => x.DRT_ID,
                        principalTable: "DROITS",
                        principalColumn: "DRT_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_ApplicationRoleId",
                table: "AspNetRoleClaims",
                column: "ApplicationRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_DROIT_ROLE_DRT_ID",
                table: "DROIT_ROLE",
                column: "DRT_ID");

            migrationBuilder.CreateIndex(
                name: "IX_DROIT_ROLE_RoleId",
                table: "DROIT_ROLE",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_DROITS_ApplicationRoleId",
                table: "DROITS",
                column: "ApplicationRoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetRoleClaims_AspNetRoles_ApplicationRoleId",
                table: "AspNetRoleClaims",
                column: "ApplicationRoleId",
                principalTable: "AspNetRoles",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetRoleClaims_AspNetRoles_ApplicationRoleId",
                table: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "DROIT_ROLE");

            migrationBuilder.DropTable(
                name: "DROITS");

            migrationBuilder.DropIndex(
                name: "IX_AspNetRoleClaims_ApplicationRoleId",
                table: "AspNetRoleClaims");

            migrationBuilder.DropColumn(
                name: "ApplicationRoleId",
                table: "AspNetRoleClaims");
        }
    }
}
