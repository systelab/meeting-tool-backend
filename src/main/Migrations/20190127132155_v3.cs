using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace main.Migrations
{
    public partial class v3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "attenders",
                table: "Rooms",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "computer",
                table: "Rooms",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "microfonia",
                table: "Rooms",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "phone",
                table: "Rooms",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "plano",
                table: "Rooms",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "planta",
                table: "Rooms",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "projector",
                table: "Rooms",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "tv",
                table: "Rooms",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "videoconference",
                table: "Rooms",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "attenders",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "computer",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "microfonia",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "phone",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "plano",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "planta",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "projector",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "tv",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "videoconference",
                table: "Rooms");
        }
    }
}
