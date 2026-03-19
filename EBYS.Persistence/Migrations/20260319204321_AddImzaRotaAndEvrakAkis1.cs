using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EBYS.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddImzaRotaAndEvrakAkis1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EvrakAkis_Evraklar_EvrakId",
                table: "EvrakAkis");

            migrationBuilder.DropForeignKey(
                name: "FK_EvrakAkis_Kullanicilar_KullaniciId",
                table: "EvrakAkis");

            migrationBuilder.DropForeignKey(
                name: "FK_Evraklar_ImzaRota_ImzaRotaId",
                table: "Evraklar");

            migrationBuilder.DropForeignKey(
                name: "FK_ImzaRotaAdimlari_ImzaRota_RotaId",
                table: "ImzaRotaAdimlari");

            migrationBuilder.DropIndex(
                name: "IX_ImzaRotaAdimlari_RotaId",
                table: "ImzaRotaAdimlari");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ImzaRota",
                table: "ImzaRota");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EvrakAkis",
                table: "EvrakAkis");

            migrationBuilder.DropColumn(
                name: "SonImzaciMi",
                table: "ImzaRotaAdimlari");

            migrationBuilder.RenameTable(
                name: "ImzaRota",
                newName: "ImzaRotalar");

            migrationBuilder.RenameTable(
                name: "EvrakAkis",
                newName: "EvrakAkislari");

            migrationBuilder.RenameColumn(
                name: "RotaId",
                table: "ImzaRotaAdimlari",
                newName: "ParafMiImzaMi");

            migrationBuilder.RenameIndex(
                name: "IX_EvrakAkis_KullaniciId",
                table: "EvrakAkislari",
                newName: "IX_EvrakAkislari_KullaniciId");

            migrationBuilder.RenameIndex(
                name: "IX_EvrakAkis_EvrakId",
                table: "EvrakAkislari",
                newName: "IX_EvrakAkislari_EvrakId");

            migrationBuilder.AddColumn<int>(
                name: "ImzaRotaId",
                table: "ImzaRotaAdimlari",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ImzaRotalar",
                table: "ImzaRotalar",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EvrakAkislari",
                table: "EvrakAkislari",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_ImzaRotaAdimlari_ImzaRotaId",
                table: "ImzaRotaAdimlari",
                column: "ImzaRotaId");

            migrationBuilder.CreateIndex(
                name: "IX_ImzaRotaAdimlari_KullaniciId",
                table: "ImzaRotaAdimlari",
                column: "KullaniciId");

            migrationBuilder.AddForeignKey(
                name: "FK_EvrakAkislari_Evraklar_EvrakId",
                table: "EvrakAkislari",
                column: "EvrakId",
                principalTable: "Evraklar",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EvrakAkislari_Kullanicilar_KullaniciId",
                table: "EvrakAkislari",
                column: "KullaniciId",
                principalTable: "Kullanicilar",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Evraklar_ImzaRotalar_ImzaRotaId",
                table: "Evraklar",
                column: "ImzaRotaId",
                principalTable: "ImzaRotalar",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ImzaRotaAdimlari_ImzaRotalar_ImzaRotaId",
                table: "ImzaRotaAdimlari",
                column: "ImzaRotaId",
                principalTable: "ImzaRotalar",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ImzaRotaAdimlari_Kullanicilar_KullaniciId",
                table: "ImzaRotaAdimlari",
                column: "KullaniciId",
                principalTable: "Kullanicilar",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EvrakAkislari_Evraklar_EvrakId",
                table: "EvrakAkislari");

            migrationBuilder.DropForeignKey(
                name: "FK_EvrakAkislari_Kullanicilar_KullaniciId",
                table: "EvrakAkislari");

            migrationBuilder.DropForeignKey(
                name: "FK_Evraklar_ImzaRotalar_ImzaRotaId",
                table: "Evraklar");

            migrationBuilder.DropForeignKey(
                name: "FK_ImzaRotaAdimlari_ImzaRotalar_ImzaRotaId",
                table: "ImzaRotaAdimlari");

            migrationBuilder.DropForeignKey(
                name: "FK_ImzaRotaAdimlari_Kullanicilar_KullaniciId",
                table: "ImzaRotaAdimlari");

            migrationBuilder.DropIndex(
                name: "IX_ImzaRotaAdimlari_ImzaRotaId",
                table: "ImzaRotaAdimlari");

            migrationBuilder.DropIndex(
                name: "IX_ImzaRotaAdimlari_KullaniciId",
                table: "ImzaRotaAdimlari");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ImzaRotalar",
                table: "ImzaRotalar");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EvrakAkislari",
                table: "EvrakAkislari");

            migrationBuilder.DropColumn(
                name: "ImzaRotaId",
                table: "ImzaRotaAdimlari");

            migrationBuilder.RenameTable(
                name: "ImzaRotalar",
                newName: "ImzaRota");

            migrationBuilder.RenameTable(
                name: "EvrakAkislari",
                newName: "EvrakAkis");

            migrationBuilder.RenameColumn(
                name: "ParafMiImzaMi",
                table: "ImzaRotaAdimlari",
                newName: "RotaId");

            migrationBuilder.RenameIndex(
                name: "IX_EvrakAkislari_KullaniciId",
                table: "EvrakAkis",
                newName: "IX_EvrakAkis_KullaniciId");

            migrationBuilder.RenameIndex(
                name: "IX_EvrakAkislari_EvrakId",
                table: "EvrakAkis",
                newName: "IX_EvrakAkis_EvrakId");

            migrationBuilder.AddColumn<bool>(
                name: "SonImzaciMi",
                table: "ImzaRotaAdimlari",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ImzaRota",
                table: "ImzaRota",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EvrakAkis",
                table: "EvrakAkis",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_ImzaRotaAdimlari_RotaId",
                table: "ImzaRotaAdimlari",
                column: "RotaId");

            migrationBuilder.AddForeignKey(
                name: "FK_EvrakAkis_Evraklar_EvrakId",
                table: "EvrakAkis",
                column: "EvrakId",
                principalTable: "Evraklar",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EvrakAkis_Kullanicilar_KullaniciId",
                table: "EvrakAkis",
                column: "KullaniciId",
                principalTable: "Kullanicilar",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Evraklar_ImzaRota_ImzaRotaId",
                table: "Evraklar",
                column: "ImzaRotaId",
                principalTable: "ImzaRota",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ImzaRotaAdimlari_ImzaRota_RotaId",
                table: "ImzaRotaAdimlari",
                column: "RotaId",
                principalTable: "ImzaRota",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
