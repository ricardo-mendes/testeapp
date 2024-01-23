using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PetAdmin.Web.Migrations
{
    public partial class LocationDistance : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PetLoverLocationClient",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Uid = table.Column<Guid>(nullable: false),
                    PetLoverId = table.Column<long>(nullable: false),
                    ClientId = table.Column<long>(nullable: false),
                    Distance = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PetLoverLocationClient", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PetLoverLocationClient_Client_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Client",
                        principalColumn: "ClientId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PetLoverLocationClient_PetLover_PetLoverId",
                        column: x => x.PetLoverId,
                        principalTable: "PetLover",
                        principalColumn: "PetLoverId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PetLoverLocationClient_ClientId",
                table: "PetLoverLocationClient",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_PetLoverLocationClient_PetLoverId",
                table: "PetLoverLocationClient",
                column: "PetLoverId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PetLoverLocationClient");
        }
    }
}
