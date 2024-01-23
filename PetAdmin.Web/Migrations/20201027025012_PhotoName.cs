using Microsoft.EntityFrameworkCore.Migrations;

namespace PetAdmin.Web.Migrations
{
    public partial class PhotoName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PhotoName",
                table: "Vaccine",
                maxLength: 180,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhotoName",
                table: "Pet",
                maxLength: 180,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhotoName",
                table: "Vaccine");

            migrationBuilder.DropColumn(
                name: "PhotoName",
                table: "Pet");
        }
    }
}
