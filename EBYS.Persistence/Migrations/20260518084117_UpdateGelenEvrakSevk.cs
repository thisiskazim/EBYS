using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EBYS.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateGelenEvrakSevk : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GelenEvrakSevkler_Kullanicilar_GonderenKullaniciId",
                table: "GelenEvrakSevkler");

            migrationBuilder.RenameColumn(
                name: "GonderenKullaniciId",
                table: "GelenEvrakSevkler",
                newName: "SevkEdenKullaniciId");

            migrationBuilder.RenameIndex(
                name: "IX_GelenEvrakSevkler_GonderenKullaniciId",
                table: "GelenEvrakSevkler",
                newName: "IX_GelenEvrakSevkler_SevkEdenKullaniciId");

            migrationBuilder.AddForeignKey(
                name: "FK_GelenEvrakSevkler_Kullanicilar_SevkEdenKullaniciId",
                table: "GelenEvrakSevkler",
                column: "SevkEdenKullaniciId",
                principalTable: "Kullanicilar",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GelenEvrakSevkler_Kullanicilar_SevkEdenKullaniciId",
                table: "GelenEvrakSevkler");

            migrationBuilder.RenameColumn(
                name: "SevkEdenKullaniciId",
                table: "GelenEvrakSevkler",
                newName: "GonderenKullaniciId");

            migrationBuilder.RenameIndex(
                name: "IX_GelenEvrakSevkler_SevkEdenKullaniciId",
                table: "GelenEvrakSevkler",
                newName: "IX_GelenEvrakSevkler_GonderenKullaniciId");

            migrationBuilder.AddForeignKey(
                name: "FK_GelenEvrakSevkler_Kullanicilar_GonderenKullaniciId",
                table: "GelenEvrakSevkler",
                column: "GonderenKullaniciId",
                principalTable: "Kullanicilar",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
