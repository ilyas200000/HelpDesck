using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Service_Incidents.Migrations
{
    /// <inheritdoc />
    public partial class categorie_table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    CategoryID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.CategoryID);
                });

            migrationBuilder.CreateTable(
                name: "Priorites",
                columns: table => new
                {
                    INCD_PRIO_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PRIO_DESC = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Priorites", x => x.INCD_PRIO_ID);
                });

            migrationBuilder.CreateTable(
                name: "Statuts",
                columns: table => new
                {
                    INCD_STAT_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    STAT_DESC = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Statuts", x => x.INCD_STAT_ID);
                });

            migrationBuilder.CreateTable(
                name: "Types",
                columns: table => new
                {
                    INCD_TYPE_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TYPE_DESC = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CategoryID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Types", x => x.INCD_TYPE_ID);
                    table.ForeignKey(
                        name: "FK_Types_Categories_CategoryID",
                        column: x => x.CategoryID,
                        principalTable: "Categories",
                        principalColumn: "CategoryID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Incidents",
                columns: table => new
                {
                    INCD_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    INCD_ENTT_SG_ID = table.Column<int>(type: "int", nullable: true),
                    INCD_TYPE_ID = table.Column<int>(type: "int", nullable: false),
                    INCD_STAT_ID = table.Column<int>(type: "int", nullable: false),
                    INCD_PRIO_ID = table.Column<int>(type: "int", nullable: false),
                    INCD_UTIL_ID = table.Column<int>(type: "int", nullable: false),
                    INCD_NUM_TICK = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    INCD_DESC = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    agn_code = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    incd_date_creation = table.Column<DateTime>(type: "datetime2", nullable: false),
                    incd_date_resolution = table.Column<DateTime>(type: "datetime2", nullable: true),
                    incd_date_cloture = table.Column<DateTime>(type: "datetime2", nullable: true),
                    incd_audit = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    pres_id = table.Column<int>(type: "int", nullable: true),
                    niveau_escalade = table.Column<int>(type: "int", nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    IsSendSms1 = table.Column<bool>(type: "bit", nullable: true),
                    IsSendSms2 = table.Column<bool>(type: "bit", nullable: true),
                    IsSendSms3 = table.Column<bool>(type: "bit", nullable: true),
                    IsSendSms4 = table.Column<bool>(type: "bit", nullable: true),
                    Motif_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Incidents", x => x.INCD_ID);
                    table.ForeignKey(
                        name: "FK_Incidents_Priorites_INCD_PRIO_ID",
                        column: x => x.INCD_PRIO_ID,
                        principalTable: "Priorites",
                        principalColumn: "INCD_PRIO_ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Incidents_Statuts_INCD_STAT_ID",
                        column: x => x.INCD_STAT_ID,
                        principalTable: "Statuts",
                        principalColumn: "INCD_STAT_ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Incidents_Types_INCD_TYPE_ID",
                        column: x => x.INCD_TYPE_ID,
                        principalTable: "Types",
                        principalColumn: "INCD_TYPE_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Incidents_INCD_PRIO_ID",
                table: "Incidents",
                column: "INCD_PRIO_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Incidents_INCD_STAT_ID",
                table: "Incidents",
                column: "INCD_STAT_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Incidents_INCD_TYPE_ID",
                table: "Incidents",
                column: "INCD_TYPE_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Types_CategoryID",
                table: "Types",
                column: "CategoryID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Incidents");

            migrationBuilder.DropTable(
                name: "Priorites");

            migrationBuilder.DropTable(
                name: "Statuts");

            migrationBuilder.DropTable(
                name: "Types");

            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}
