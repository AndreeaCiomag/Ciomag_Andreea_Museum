using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ciomag_Andreea_Museum.Migrations
{
    public partial class Second : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Gallery",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Exhibition",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Exhibit",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Gallery");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Exhibition");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Exhibit");
        }
    }
}
