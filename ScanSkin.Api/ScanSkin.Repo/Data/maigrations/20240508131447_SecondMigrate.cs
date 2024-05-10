using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ScanSkin.Repo.Data.maigrations
{
    public partial class SecondMigrate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Doctor_Id",
                table: "Appointments",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Doctor_Id",
                table: "Appointments");
        }
    }
}
