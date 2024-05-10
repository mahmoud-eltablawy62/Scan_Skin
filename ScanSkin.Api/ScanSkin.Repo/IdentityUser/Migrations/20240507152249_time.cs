using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ScanSkin.Repo.IdentityUser.Migrations
{
    public partial class time : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndDay",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "StartDay",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<TimeSpan>(
                name: "DurationTime",
                table: "AspNetUsers",
                type: "time",
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "EndWork",
                table: "AspNetUsers",
                type: "time",
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "StartWork",
                table: "AspNetUsers",
                type: "time",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DurationTime",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "EndWork",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "StartWork",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDay",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDay",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: true);
        }
    }
}
