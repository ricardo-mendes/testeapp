using Microsoft.EntityFrameworkCore.Migrations;

namespace PetAdmin.Web.Migrations
{
    public partial class PhotoUrl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PhotoUrl",
                table: "Vaccine",
                maxLength: 300,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhotoUrl",
                table: "Pet",
                maxLength: 300,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhotoUrl",
                table: "Vaccine");

            migrationBuilder.DropColumn(
                name: "PhotoUrl",
                table: "Pet");
        }
    }
}
