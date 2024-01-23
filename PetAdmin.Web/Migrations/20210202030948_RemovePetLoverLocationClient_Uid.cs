using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PetAdmin.Web.Migrations
{
    public partial class RemovePetLoverLocationClient_Uid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Uid",
                table: "PetLoverLocationClient");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "Uid",
                table: "PetLoverLocationClient",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }
    }
}
