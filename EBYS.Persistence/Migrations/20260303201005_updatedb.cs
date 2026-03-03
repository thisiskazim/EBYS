using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace EBYS.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class updatedb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BaseKurumId",
                table: "Roller",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BaseKurumId",
                table: "Muhataplar",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BaseKurumId",
                table: "Kullanicilar",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "SifreHash",
                table: "Kullanicilar",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "BaseKurumId",
                table: "EvrakMuhataplari",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BaseKurumId",
                table: "Evraklar",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ImzaAltindaOlanIcerik",
                table: "Evraklar",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "BaseKurumId",
                table: "EvrakIlgiler",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BaseKurumId",
                table: "EvrakEkler",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "BaseKurums",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    KurumAdi = table.Column<string>(type: "text", nullable: false),
                    VergiNo = table.Column<string>(type: "text", nullable: false),
                    KurumKodu = table.Column<string>(type: "text", nullable: false),
                    DetsisNo = table.Column<string>(type: "text", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    BaseKurumId = table.Column<int>(type: "integer", nullable: false),
                    creat_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    isDelete = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BaseKurums", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Kullanicilar_BaseKurumId",
                table: "Kullanicilar",
                column: "BaseKurumId");

            migrationBuilder.AddForeignKey(
                name: "FK_Kullanicilar_BaseKurums_BaseKurumId",
                table: "Kullanicilar",
                column: "BaseKurumId",
                principalTable: "BaseKurums",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Kullanicilar_BaseKurums_BaseKurumId",
                table: "Kullanicilar");

            migrationBuilder.DropTable(
                name: "BaseKurums");

            migrationBuilder.DropIndex(
                name: "IX_Kullanicilar_BaseKurumId",
                table: "Kullanicilar");

            migrationBuilder.DropColumn(
                name: "BaseKurumId",
                table: "Roller");

            migrationBuilder.DropColumn(
                name: "BaseKurumId",
                table: "Muhataplar");

            migrationBuilder.DropColumn(
                name: "BaseKurumId",
                table: "Kullanicilar");

            migrationBuilder.DropColumn(
                name: "SifreHash",
                table: "Kullanicilar");

            migrationBuilder.DropColumn(
                name: "BaseKurumId",
                table: "EvrakMuhataplari");

            migrationBuilder.DropColumn(
                name: "BaseKurumId",
                table: "Evraklar");

            migrationBuilder.DropColumn(
                name: "ImzaAltindaOlanIcerik",
                table: "Evraklar");

            migrationBuilder.DropColumn(
                name: "BaseKurumId",
                table: "EvrakIlgiler");

            migrationBuilder.DropColumn(
                name: "BaseKurumId",
                table: "EvrakEkler");
        }
    }
}
