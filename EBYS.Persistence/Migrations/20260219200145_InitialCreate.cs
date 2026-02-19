using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace EBYS.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Muhataplar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Adi = table.Column<string>(type: "text", nullable: false),
                    Telefon = table.Column<string>(type: "text", nullable: false),
                    EPosta = table.Column<string>(type: "text", nullable: true),
                    Adress = table.Column<string>(type: "text", nullable: false),
                    MuhatapTipi = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: false),
                    KimlikNo = table.Column<string>(type: "text", nullable: true),
                    KepAdresi = table.Column<string>(type: "text", nullable: true),
                    TesisNo = table.Column<string>(type: "text", nullable: true),
                    DetsisNo = table.Column<string>(type: "text", nullable: true),
                    KurumKodu = table.Column<string>(type: "text", nullable: true),
                    VergiNo = table.Column<string>(type: "text", nullable: true),
                    VergiDairesi = table.Column<string>(type: "text", nullable: true),
                    MersisNo = table.Column<string>(type: "text", nullable: true),
                    creat_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    isDelete = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Muhataplar", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roller",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RolAdi = table.Column<string>(type: "text", nullable: false),
                    creat_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    isDelete = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roller", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Kullanicilar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Ad = table.Column<string>(type: "text", nullable: false),
                    Soyad = table.Column<string>(type: "text", nullable: false),
                    KimlikNo = table.Column<string>(type: "text", nullable: false),
                    RolId = table.Column<int>(type: "integer", nullable: false),
                    creat_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    isDelete = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kullanicilar", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Kullanicilar_Roller_RolId",
                        column: x => x.RolId,
                        principalTable: "Roller",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Evraklar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Konu = table.Column<string>(type: "text", nullable: false),
                    Icerik = table.Column<string>(type: "text", nullable: false),
                    EvrakSayisi = table.Column<string>(type: "text", nullable: false),
                    IsGelenEvrak = table.Column<bool>(type: "boolean", nullable: false),
                    Durum = table.Column<int>(type: "integer", nullable: false),
                    OlusturanId = table.Column<int>(type: "integer", nullable: false),
                    BekleyenRolId = table.Column<int>(type: "integer", nullable: true),
                    creat_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    isDelete = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Evraklar", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Evraklar_Kullanicilar_OlusturanId",
                        column: x => x.OlusturanId,
                        principalTable: "Kullanicilar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Evraklar_Roller_BekleyenRolId",
                        column: x => x.BekleyenRolId,
                        principalTable: "Roller",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EvrakEkler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EvrakId = table.Column<int>(type: "integer", nullable: true),
                    EkAdi = table.Column<string>(type: "text", nullable: true),
                    DosyaYolu = table.Column<string>(type: "text", nullable: true),
                    creat_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    isDelete = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EvrakEkler", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EvrakEkler_Evraklar_EvrakId",
                        column: x => x.EvrakId,
                        principalTable: "Evraklar",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EvrakIlgiler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EvrakId = table.Column<int>(type: "integer", nullable: true),
                    IlgiMetni = table.Column<string>(type: "text", nullable: true),
                    creat_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    isDelete = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EvrakIlgiler", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EvrakIlgiler_Evraklar_EvrakId",
                        column: x => x.EvrakId,
                        principalTable: "Evraklar",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EvrakMuhataplari",
                columns: table => new
                {
                    EvrakId = table.Column<int>(type: "integer", nullable: false),
                    MuhatapId = table.Column<int>(type: "integer", nullable: false),
                    IsBilgi = table.Column<bool>(type: "boolean", nullable: false),
                    Id = table.Column<int>(type: "integer", nullable: false),
                    creat_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    isDelete = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EvrakMuhataplari", x => new { x.EvrakId, x.MuhatapId });
                    table.ForeignKey(
                        name: "FK_EvrakMuhataplari_Evraklar_EvrakId",
                        column: x => x.EvrakId,
                        principalTable: "Evraklar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EvrakMuhataplari_Muhataplar_MuhatapId",
                        column: x => x.MuhatapId,
                        principalTable: "Muhataplar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EvrakEkler_EvrakId",
                table: "EvrakEkler",
                column: "EvrakId");

            migrationBuilder.CreateIndex(
                name: "IX_EvrakIlgiler_EvrakId",
                table: "EvrakIlgiler",
                column: "EvrakId");

            migrationBuilder.CreateIndex(
                name: "IX_Evraklar_BekleyenRolId",
                table: "Evraklar",
                column: "BekleyenRolId");

            migrationBuilder.CreateIndex(
                name: "IX_Evraklar_OlusturanId",
                table: "Evraklar",
                column: "OlusturanId");

            migrationBuilder.CreateIndex(
                name: "IX_EvrakMuhataplari_MuhatapId",
                table: "EvrakMuhataplari",
                column: "MuhatapId");

            migrationBuilder.CreateIndex(
                name: "IX_Kullanicilar_RolId",
                table: "Kullanicilar",
                column: "RolId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EvrakEkler");

            migrationBuilder.DropTable(
                name: "EvrakIlgiler");

            migrationBuilder.DropTable(
                name: "EvrakMuhataplari");

            migrationBuilder.DropTable(
                name: "Evraklar");

            migrationBuilder.DropTable(
                name: "Muhataplar");

            migrationBuilder.DropTable(
                name: "Kullanicilar");

            migrationBuilder.DropTable(
                name: "Roller");
        }
    }
}
