using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ciomag_Andreea_Museum.Migrations
{
    public partial class Third : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.CreateTable(
                name: "VisitedExhibition",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ClientID = table.Column<int>(type: "INTEGER", nullable: false),
                    ExhibitionID = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VisitedExhibition", x => x.ID);
                    table.ForeignKey(
                        name: "FK_VisitedExhibition_Client_ClientID",
                        column: x => x.ClientID,
                        principalTable: "Client",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VisitedExhibition_Exhibition_ExhibitionID",
                        column: x => x.ExhibitionID,
                        principalTable: "Exhibition",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClientExhibition_ExhibitionsID",
                table: "ClientExhibition",
                column: "ExhibitionsID");

            migrationBuilder.CreateIndex(
                name: "IX_VisitedExhibition_ClientID",
                table: "VisitedExhibition",
                column: "ClientID");

            migrationBuilder.CreateIndex(
                name: "IX_VisitedExhibition_ExhibitionID",
                table: "VisitedExhibition",
                column: "ExhibitionID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClientExhibition");

            migrationBuilder.DropTable(
                name: "VisitedExhibition");
        }
    }
}
