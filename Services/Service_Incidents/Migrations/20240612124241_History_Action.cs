using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Service_Incidents.Migrations
{
    /// <inheritdoc />
    public partial class History_Action : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "IncidentHistories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IncidentId = table.Column<int>(type: "int", nullable: false),
                    OldStatusId = table.Column<int>(type: "int", nullable: false),
                    NewStatusId = table.Column<int>(type: "int", nullable: false),
                    ChangeDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ChangedBy = table.Column<int>(type: "int", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IncidentHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IncidentHistories_Incidents_IncidentId",
                        column: x => x.IncidentId,
                        principalTable: "Incidents",
                        principalColumn: "INCD_ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_IncidentHistories_Statuts_NewStatusId",
                        column: x => x.NewStatusId,
                        principalTable: "Statuts",
                        principalColumn: "INCD_STAT_ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IncidentHistories_Statuts_OldStatusId",
                        column: x => x.OldStatusId,
                        principalTable: "Statuts",
                        principalColumn: "INCD_STAT_ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_IncidentHistories_IncidentId",
                table: "IncidentHistories",
                column: "IncidentId");

            migrationBuilder.CreateIndex(
                name: "IX_IncidentHistories_NewStatusId",
                table: "IncidentHistories",
                column: "NewStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_IncidentHistories_OldStatusId",
                table: "IncidentHistories",
                column: "OldStatusId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IncidentHistories");
        }
    }
}
