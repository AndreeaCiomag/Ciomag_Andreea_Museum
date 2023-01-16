using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ciomag_Andreea_Museum.Migrations
{
    public partial class ThirdComplete : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClientExhibition");

            migrationBuilder.DropPrimaryKey(
                name: "PK_VisitedExhibition",
                table: "VisitedExhibition");

            migrationBuilder.DropIndex(
                name: "IX_VisitedExhibition_ClientID",
                table: "VisitedExhibition");

            migrationBuilder.DropColumn(
                name: "ID",
                table: "VisitedExhibition");

            migrationBuilder.AddPrimaryKey(
                name: "PK_VisitedExhibition",
                table: "VisitedExhibition",
                columns: new[] { "ClientID", "ExhibitionID" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_VisitedExhibition",
                table: "VisitedExhibition");

            migrationBuilder.AddColumn<int>(
                name: "ID",
                table: "VisitedExhibition",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0)
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_VisitedExhibition",
                table: "VisitedExhibition",
                column: "ID");

            migrationBuilder.CreateTable(
                name: "ClientExhibition",
                columns: table => new
                {
                    ClientsID = table.Column<int>(type: "INTEGER", nullable: false),
                    ExhibitionsID = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientExhibition", x => new { x.ClientsID, x.ExhibitionsID });
                    table.ForeignKey(
                        name: "FK_ClientExhibition_Client_ClientsID",
                        column: x => x.ClientsID,
                        principalTable: "Client",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClientExhibition_Exhibition_ExhibitionsID",
                        column: x => x.ExhibitionsID,
                        principalTable: "Exhibition",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_VisitedExhibition_ClientID",
                table: "VisitedExhibition",
                column: "ClientID");

            migrationBuilder.CreateIndex(
                name: "IX_ClientExhibition_ExhibitionsID",
                table: "ClientExhibition",
                column: "ExhibitionsID");
        }
    }
}
