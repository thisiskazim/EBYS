using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EBYS.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class updatemigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GelenEvrakSevkler_Kullanicilar_KaydedenKullaniciId",
                table: "GelenEvrakSevkler");

            migrationBuilder.RenameColumn(
                name: "KaydedenKullaniciId",
                table: "GelenEvrakSevkler",
                newName: "GonderenKullaniciId");

            migrationBuilder.RenameIndex(
                name: "IX_GelenEvrakSevkler_KaydedenKullaniciId",
                table: "GelenEvrakSevkler",
                newName: "IX_GelenEvrakSevkler_GonderenKullaniciId");

            migrationBuilder.AddColumn<int>(
                name: "OlusturanId",
                table: "GelenEvraklar",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_GelenEvraklar_OlusturanId",
                table: "GelenEvraklar",
                column: "OlusturanId");

            migrationBuilder.AddForeignKey(
                name: "FK_GelenEvraklar_Kullanicilar_OlusturanId",
                table: "GelenEvraklar",
                column: "OlusturanId",
                principalTable: "Kullanicilar",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GelenEvrakSevkler_Kullanicilar_GonderenKullaniciId",
                table: "GelenEvrakSevkler",
                column: "GonderenKullaniciId",
                principalTable: "Kullanicilar",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GelenEvraklar_Kullanicilar_OlusturanId",
                table: "GelenEvraklar");

            migrationBuilder.DropForeignKey(
                name: "FK_GelenEvrakSevkler_Kullanicilar_GonderenKullaniciId",
                table: "GelenEvrakSevkler");

            migrationBuilder.DropIndex(
                name: "IX_GelenEvraklar_OlusturanId",
                table: "GelenEvraklar");

            migrationBuilder.DropColumn(
                name: "OlusturanId",
                table: "GelenEvraklar");

            migrationBuilder.RenameColumn(
                name: "GonderenKullaniciId",
                table: "GelenEvrakSevkler",
                newName: "KaydedenKullaniciId");

            migrationBuilder.RenameIndex(
                name: "IX_GelenEvrakSevkler_GonderenKullaniciId",
                table: "GelenEvrakSevkler",
                newName: "IX_GelenEvrakSevkler_KaydedenKullaniciId");

            migrationBuilder.AddForeignKey(
                name: "FK_GelenEvrakSevkler_Kullanicilar_KaydedenKullaniciId",
                table: "GelenEvrakSevkler",
                column: "KaydedenKullaniciId",
                principalTable: "Kullanicilar",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
