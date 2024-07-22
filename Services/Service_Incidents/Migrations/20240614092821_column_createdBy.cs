using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Service_Incidents.Migrations
{
    /// <inheritdoc />
    public partial class column_createdBy : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "createdBy",
                table: "Incidents",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "createdBy",
                table: "Incidents");
        }
    }
}
