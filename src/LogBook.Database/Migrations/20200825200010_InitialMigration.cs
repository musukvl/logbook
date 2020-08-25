using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LogBook.Database.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "routes",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Date = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_routes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "waypoints",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    RouteId = table.Column<Guid>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_waypoints", x => x.Id);
                    table.ForeignKey(
                        name: "FK_waypoints_routes_RouteId",
                        column: x => x.RouteId,
                        principalTable: "routes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_waypoints_RouteId",
                table: "waypoints",
                column: "RouteId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "waypoints");

            migrationBuilder.DropTable(
                name: "routes");
        }
    }
}
