using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace EBYS.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class GelenEvrakBaslangic : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GelenEvraklar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Konu = table.Column<string>(type: "text", nullable: false),
                    EvrakSayisi = table.Column<string>(type: "text", nullable: false),
                    EvrakTarihi = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    DefterTarihi = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    KayitNo = table.Column<string>(type: "text", nullable: false),
                    DilekceMi = table.Column<bool>(type: "boolean", nullable: false),
                    KonuKodu = table.Column<string>(type: "text", nullable: false),
                    GonderenMuhatapId = table.Column<int>(type: "integer", nullable: false),
                    GizlilikDerecesi = table.Column<int>(type: "integer", nullable: false),
                    IvedilikDerecesi = table.Column<int>(type: "integer", nullable: false),
                    CevapIstenenTarih = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    BaseKurumId = table.Column<int>(type: "integer", nullable: false),
                    creat_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    isDelete = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GelenEvraklar", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GelenEvrakEkler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    GelenEvrakId = table.Column<int>(type: "integer", nullable: true),
                    Ad = table.Column<string>(type: "text", nullable: true),
                    DosyaVerisi = table.Column<byte[]>(type: "bytea", nullable: true),
                    DosyaUzantisi = table.Column<string>(type: "text", nullable: true),
                    MimeType = table.Column<string>(type: "text", nullable: true),
                    BaseKurumId = table.Column<int>(type: "integer", nullable: false),
                    creat_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    isDelete = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GelenEvrakEkler", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GelenEvrakEkler_GelenEvraklar_GelenEvrakId",
                        column: x => x.GelenEvrakId,
                        principalTable: "GelenEvraklar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GelenEvrakIlgileri",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    GelenEvrakId = table.Column<int>(type: "integer", nullable: true),
                    IlgiMetni = table.Column<string>(type: "text", nullable: true),
                    BaseKurumId = table.Column<int>(type: "integer", nullable: false),
                    creat_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    isDelete = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GelenEvrakIlgileri", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GelenEvrakIlgileri_GelenEvraklar_GelenEvrakId",
                        column: x => x.GelenEvrakId,
                        principalTable: "GelenEvraklar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GelenEvrakSevkler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    GelenEvrakId = table.Column<int>(type: "integer", nullable: false),
                    GonderenUserId = table.Column<int>(type: "integer", nullable: false),
                    AliciUserId = table.Column<int>(type: "integer", nullable: true),
                    Aciklama = table.Column<string>(type: "text", nullable: false),
                    SevkTarihi = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    OkunmaTarihi = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    BaseKurumId = table.Column<int>(type: "integer", nullable: false),
                    creat_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    isDelete = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GelenEvrakSevkler", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GelenEvrakSevkler_GelenEvraklar_GelenEvrakId",
                        column: x => x.GelenEvrakId,
                        principalTable: "GelenEvraklar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GelenEvrakEkler_GelenEvrakId",
                table: "GelenEvrakEkler",
                column: "GelenEvrakId");

            migrationBuilder.CreateIndex(
                name: "IX_GelenEvrakIlgileri_GelenEvrakId",
                table: "GelenEvrakIlgileri",
                column: "GelenEvrakId");

            migrationBuilder.CreateIndex(
                name: "IX_GelenEvraklar_KayitNo",
                table: "GelenEvraklar",
                column: "KayitNo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GelenEvrakSevkler_GelenEvrakId",
                table: "GelenEvrakSevkler",
                column: "GelenEvrakId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GelenEvrakEkler");

            migrationBuilder.DropTable(
                name: "GelenEvrakIlgileri");

            migrationBuilder.DropTable(
                name: "GelenEvrakSevkler");

            migrationBuilder.DropTable(
                name: "GelenEvraklar");
        }
    }
}
