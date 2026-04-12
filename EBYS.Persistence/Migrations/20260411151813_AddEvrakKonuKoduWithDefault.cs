using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace EBYS.Persistence.Migrations
{
    public partial class AddEvrakKonuKoduWithDefault : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // 1. Önce mevcut kısıtlamaları (Foreign Key) kaldırıyoruz (Güncelleme için)
            migrationBuilder.DropForeignKey(name: "FK_Evraklar_ImzaRotalar_ImzaRotaId", table: "Evraklar");
            migrationBuilder.DropForeignKey(name: "FK_Evraklar_Kullanicilar_OlusturanId", table: "Evraklar");

            // 2. Gereksiz kolonu siliyoruz
            migrationBuilder.DropColumn(name: "DosyaYolu", table: "EvrakEkler");

            // 3. Konu Kodları tablosunu oluşturuyoruz
            migrationBuilder.CreateTable(
                name: "EvrakKonuKodlari",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    KodNumber = table.Column<string>(type: "text", nullable: false),
                    KodAdi = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EvrakKonuKodlari", x => x.Id);
                });

            // 4. Varsayılan kayıt ekliyoruz (ID: 1)
            migrationBuilder.Sql("INSERT INTO \"EvrakKonuKodlari\" (\"Id\", \"KodNumber\", \"KodAdi\") VALUES (1, '000.00', 'Tanımlanmamış / Eski Evrak')");

            // 5. Evraklar tablosuna KonuKoduId kolonunu 1 varsayılan değeriyle ekliyoruz
            migrationBuilder.AddColumn<int>(
                name: "KonuKoduId",
                table: "Evraklar",
                type: "integer",
                nullable: false,
                defaultValue: 1);

            // 6. Index oluşturuyoruz
            migrationBuilder.CreateIndex(
                name: "IX_Evraklar_KonuKoduId",
                table: "Evraklar",
                column: "KonuKoduId");

            // 7. Yeni Foreign Key'i (Konu Kodu) ekliyoruz
            migrationBuilder.AddForeignKey(
                name: "FK_Evraklar_EvrakKonuKodlari_KonuKoduId",
                table: "Evraklar",
                column: "KonuKoduId",
                principalTable: "EvrakKonuKodlari",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            // 8. Diğer Foreign Key'leri "Restrict" olacak şekilde geri bağlıyoruz
            migrationBuilder.AddForeignKey(
                name: "FK_Evraklar_ImzaRotalar_ImzaRotaId",
                table: "Evraklar",
                column: "ImzaRotaId",
                principalTable: "ImzaRotalar",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Evraklar_Kullanicilar_OlusturanId",
                table: "Evraklar",
                column: "OlusturanId",
                principalTable: "Kullanicilar",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(name: "FK_Evraklar_EvrakKonuKodlari_KonuKoduId", table: "Evraklar");
            migrationBuilder.DropForeignKey(name: "FK_Evraklar_ImzaRotalar_ImzaRotaId", table: "Evraklar");
            migrationBuilder.DropForeignKey(name: "FK_Evraklar_Kullanicilar_OlusturanId", table: "Evraklar");

            migrationBuilder.DropTable(name: "EvrakKonuKodlari");
            migrationBuilder.DropIndex(name: "IX_Evraklar_KonuKoduId", table: "Evraklar");
            migrationBuilder.DropColumn(name: "KonuKoduId", table: "Evraklar");

            migrationBuilder.AddColumn<string>(name: "DosyaYolu", table: "EvrakEkler", type: "text", nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Evraklar_ImzaRotalar_ImzaRotaId",
                table: "Evraklar",
                column: "ImzaRotaId",
                principalTable: "ImzaRotalar",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Evraklar_Kullanicilar_OlusturanId",
                table: "Evraklar",
                column: "OlusturanId",
                principalTable: "Kullanicilar",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}