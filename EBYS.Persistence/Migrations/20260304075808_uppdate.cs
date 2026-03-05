using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EBYS.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class uppdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Kullanicilar",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "BaseKurums",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Roller",
                keyColumn: "Id",
                keyValue: 1);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "BaseKurums",
                columns: new[] { "Id", "BaseKurumId", "DetsisNo", "IsActive", "KurumAdi", "KurumKodu", "VergiNo", "creat_time", "isDelete" },
                values: new object[] { 1, 1, "DTS123456", true, "EBYS Genel Müdürlüğü", "EGM001", "1234567890", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false });

            migrationBuilder.InsertData(
                table: "Roller",
                columns: new[] { "Id", "BaseKurumId", "RolAdi", "creat_time", "isDelete" },
                values: new object[] { 1, 1, "Sistem Yöneticisi", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false });

            migrationBuilder.InsertData(
                table: "Kullanicilar",
                columns: new[] { "Id", "Ad", "BaseKurumId", "KimlikNo", "RolId", "SifreHash", "Soyad", "creat_time", "isDelete" },
                values: new object[] { 1, "Kazim", 1, "11111111111", 1, "123456", "U", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false });
        }
    }
}
