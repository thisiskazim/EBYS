using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EBYS.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class gelenevrakpentityupdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "GonderenUserId",
                table: "GelenEvrakSevkler",
                newName: "KaydedenKullaniciId");

            migrationBuilder.RenameColumn(
                name: "AliciUserId",
                table: "GelenEvrakSevkler",
                newName: "AlanKullaniciId");

            migrationBuilder.RenameColumn(
                name: "GonderenMuhatapId",
                table: "GelenEvraklar",
                newName: "MuhatapId");

            migrationBuilder.AlterColumn<string>(
                name: "Aciklama",
                table: "GelenEvrakSevkler",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.CreateIndex(
                name: "IX_GelenEvrakSevkler_AlanKullaniciId",
                table: "GelenEvrakSevkler",
                column: "AlanKullaniciId");

            migrationBuilder.CreateIndex(
                name: "IX_GelenEvrakSevkler_KaydedenKullaniciId",
                table: "GelenEvrakSevkler",
                column: "KaydedenKullaniciId");

            migrationBuilder.CreateIndex(
                name: "IX_GelenEvraklar_MuhatapId",
                table: "GelenEvraklar",
                column: "MuhatapId");

            migrationBuilder.AddForeignKey(
                name: "FK_GelenEvraklar_Muhataplar_MuhatapId",
                table: "GelenEvraklar",
                column: "MuhatapId",
                principalTable: "Muhataplar",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GelenEvrakSevkler_Kullanicilar_AlanKullaniciId",
                table: "GelenEvrakSevkler",
                column: "AlanKullaniciId",
                principalTable: "Kullanicilar",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GelenEvrakSevkler_Kullanicilar_KaydedenKullaniciId",
                table: "GelenEvrakSevkler",
                column: "KaydedenKullaniciId",
                principalTable: "Kullanicilar",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GelenEvraklar_Muhataplar_MuhatapId",
                table: "GelenEvraklar");

            migrationBuilder.DropForeignKey(
                name: "FK_GelenEvrakSevkler_Kullanicilar_AlanKullaniciId",
                table: "GelenEvrakSevkler");

            migrationBuilder.DropForeignKey(
                name: "FK_GelenEvrakSevkler_Kullanicilar_KaydedenKullaniciId",
                table: "GelenEvrakSevkler");

            migrationBuilder.DropIndex(
                name: "IX_GelenEvrakSevkler_AlanKullaniciId",
                table: "GelenEvrakSevkler");

            migrationBuilder.DropIndex(
                name: "IX_GelenEvrakSevkler_KaydedenKullaniciId",
                table: "GelenEvrakSevkler");

            migrationBuilder.DropIndex(
                name: "IX_GelenEvraklar_MuhatapId",
                table: "GelenEvraklar");

            migrationBuilder.RenameColumn(
                name: "KaydedenKullaniciId",
                table: "GelenEvrakSevkler",
                newName: "GonderenUserId");

            migrationBuilder.RenameColumn(
                name: "AlanKullaniciId",
                table: "GelenEvrakSevkler",
                newName: "AliciUserId");

            migrationBuilder.RenameColumn(
                name: "MuhatapId",
                table: "GelenEvraklar",
                newName: "GonderenMuhatapId");

            migrationBuilder.AlterColumn<string>(
                name: "Aciklama",
                table: "GelenEvrakSevkler",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);
        }
    }
}
