using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EBYS.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class CascadeDeleteInit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EvrakEkler_Evraklar_EvrakId",
                table: "EvrakEkler");

            migrationBuilder.DropForeignKey(
                name: "FK_EvrakIlgiler_Evraklar_EvrakId",
                table: "EvrakIlgiler");

            migrationBuilder.AddForeignKey(
                name: "FK_EvrakEkler_Evraklar_EvrakId",
                table: "EvrakEkler",
                column: "EvrakId",
                principalTable: "Evraklar",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EvrakIlgiler_Evraklar_EvrakId",
                table: "EvrakIlgiler",
                column: "EvrakId",
                principalTable: "Evraklar",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EvrakEkler_Evraklar_EvrakId",
                table: "EvrakEkler");

            migrationBuilder.DropForeignKey(
                name: "FK_EvrakIlgiler_Evraklar_EvrakId",
                table: "EvrakIlgiler");

            migrationBuilder.AddForeignKey(
                name: "FK_EvrakEkler_Evraklar_EvrakId",
                table: "EvrakEkler",
                column: "EvrakId",
                principalTable: "Evraklar",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_EvrakIlgiler_Evraklar_EvrakId",
                table: "EvrakIlgiler",
                column: "EvrakId",
                principalTable: "Evraklar",
                principalColumn: "Id");
        }
    }
}
