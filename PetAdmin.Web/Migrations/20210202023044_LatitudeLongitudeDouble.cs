using Microsoft.EntityFrameworkCore.Migrations;

namespace PetAdmin.Web.Migrations
{
    public partial class LatitudeLongitudeDouble : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "Longitue",
                table: "Location",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,6)");

            migrationBuilder.AlterColumn<double>(
                name: "Latitude",
                table: "Location",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,6)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Longitue",
                table: "Location",
                type: "decimal(10,6)",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<decimal>(
                name: "Latitude",
                table: "Location",
                type: "decimal(10,6)",
                nullable: false,
                oldClrType: typeof(double));
        }
    }
}
