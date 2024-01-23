using Microsoft.EntityFrameworkCore.Migrations;

namespace PetAdmin.Web.Migrations
{
    public partial class ClientPhoto : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PhotoName",
                table: "Client",
                maxLength: 180,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhotoUrl",
                table: "Client",
                maxLength: 300,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhotoName",
                table: "Client");

            migrationBuilder.DropColumn(
                name: "PhotoUrl",
                table: "Client");
        }
    }
}
