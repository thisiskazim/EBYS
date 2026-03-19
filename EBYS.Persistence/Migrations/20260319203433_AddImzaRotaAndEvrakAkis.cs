using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace EBYS.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddImzaRotaAndEvrakAkis : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Evraklar_Roller_BekleyenRolId",
                table: "Evraklar");

            migrationBuilder.DropIndex(
                name: "IX_Evraklar_BekleyenRolId",
                table: "Evraklar");

            migrationBuilder.DropColumn(
                name: "BekleyenRolId",
                table: "Evraklar");

            migrationBuilder.AlterColumn<DateTime>(
                name: "creat_time",
                table: "Roller",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "creat_time",
                table: "Muhataplar",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "creat_time",
                table: "Kullanicilar",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "creat_time",
                table: "EvrakMuhataplari",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "creat_time",
                table: "Evraklar",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AddColumn<int>(
                name: "ImzaRotaId",
                table: "Evraklar",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<DateTime>(
                name: "creat_time",
                table: "EvrakIlgiler",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "creat_time",
                table: "EvrakEkler",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "creat_time",
                table: "BaseKurums",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.CreateTable(
                name: "EvrakAkis",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EvrakId = table.Column<int>(type: "integer", nullable: false),
                    KullaniciId = table.Column<int>(type: "integer", nullable: false),
                    SiraNo = table.Column<int>(type: "integer", nullable: false),
                    ParafMiImzaMi = table.Column<int>(type: "integer", nullable: false),
                    SiradakiMi = table.Column<bool>(type: "boolean", nullable: false),
                    AdimDurumu = table.Column<int>(type: "integer", nullable: false),
                    Not = table.Column<string>(type: "text", nullable: true),
                    BaseKurumId = table.Column<int>(type: "integer", nullable: false),
                    creat_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    isDelete = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EvrakAkis", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EvrakAkis_Evraklar_EvrakId",
                        column: x => x.EvrakId,
                        principalTable: "Evraklar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EvrakAkis_Kullanicilar_KullaniciId",
                        column: x => x.KullaniciId,
                        principalTable: "Kullanicilar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ImzaRota",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RotaAdi = table.Column<string>(type: "text", nullable: false),
                    BaseKurumId = table.Column<int>(type: "integer", nullable: false),
                    creat_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    isDelete = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImzaRota", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ImzaRotaAdimlari",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RotaId = table.Column<int>(type: "integer", nullable: false),
                    KullaniciId = table.Column<int>(type: "integer", nullable: false),
                    SiraNo = table.Column<int>(type: "integer", nullable: false),
                    SonImzaciMi = table.Column<bool>(type: "boolean", nullable: false),
                    BaseKurumId = table.Column<int>(type: "integer", nullable: false),
                    creat_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    isDelete = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImzaRotaAdimlari", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ImzaRotaAdimlari_ImzaRota_RotaId",
                        column: x => x.RotaId,
                        principalTable: "ImzaRota",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Evraklar_ImzaRotaId",
                table: "Evraklar",
                column: "ImzaRotaId");

            migrationBuilder.CreateIndex(
                name: "IX_EvrakAkis_EvrakId",
                table: "EvrakAkis",
                column: "EvrakId");

            migrationBuilder.CreateIndex(
                name: "IX_EvrakAkis_KullaniciId",
                table: "EvrakAkis",
                column: "KullaniciId");

            migrationBuilder.CreateIndex(
                name: "IX_ImzaRotaAdimlari_RotaId",
                table: "ImzaRotaAdimlari",
                column: "RotaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Evraklar_ImzaRota_ImzaRotaId",
                table: "Evraklar",
                column: "ImzaRotaId",
                principalTable: "ImzaRota",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Evraklar_ImzaRota_ImzaRotaId",
                table: "Evraklar");

            migrationBuilder.DropTable(
                name: "EvrakAkis");

            migrationBuilder.DropTable(
                name: "ImzaRotaAdimlari");

            migrationBuilder.DropTable(
                name: "ImzaRota");

            migrationBuilder.DropIndex(
                name: "IX_Evraklar_ImzaRotaId",
                table: "Evraklar");

            migrationBuilder.DropColumn(
                name: "ImzaRotaId",
                table: "Evraklar");

            migrationBuilder.AlterColumn<DateTime>(
                name: "creat_time",
                table: "Roller",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "creat_time",
                table: "Muhataplar",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "creat_time",
                table: "Kullanicilar",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "creat_time",
                table: "EvrakMuhataplari",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "creat_time",
                table: "Evraklar",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AddColumn<int>(
                name: "BekleyenRolId",
                table: "Evraklar",
                type: "integer",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "creat_time",
                table: "EvrakIlgiler",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "creat_time",
                table: "EvrakEkler",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "creat_time",
                table: "BaseKurums",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.CreateIndex(
                name: "IX_Evraklar_BekleyenRolId",
                table: "Evraklar",
                column: "BekleyenRolId");

            migrationBuilder.AddForeignKey(
                name: "FK_Evraklar_Roller_BekleyenRolId",
                table: "Evraklar",
                column: "BekleyenRolId",
                principalTable: "Roller",
                principalColumn: "Id");
        }
    }
}
