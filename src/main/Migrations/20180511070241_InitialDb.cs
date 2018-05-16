using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace main.Migrations
{
    public partial class InitialDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CheckUpdates",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Dob = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CheckUpdates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Meetings",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    EndDateTime = table.Column<DateTime>(nullable: false),
                    IdLotus = table.Column<string>(nullable: true),
                    IdRoom = table.Column<int>(nullable: false),
                    Link = table.Column<string>(nullable: true),
                    StartDateTime = table.Column<DateTime>(nullable: false),
                    Who = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Meetings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Rooms",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Deleted = table.Column<bool>(nullable: false),
                    IdLotus = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rooms", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CheckUpdates");

            migrationBuilder.DropTable(
                name: "Meetings");

            migrationBuilder.DropTable(
                name: "Rooms");
        }
    }
}
