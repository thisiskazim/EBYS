using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EBYS.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class TestData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "BaseKurums",
                keyColumn: "Id",
                keyValue: 1,
                column: "creat_time",
                value: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Kullanicilar",
                keyColumn: "Id",
                keyValue: 1,
                column: "creat_time",
                value: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Roller",
                keyColumn: "Id",
                keyValue: 1,
                column: "creat_time",
                value: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "BaseKurums",
                keyColumn: "Id",
                keyValue: 1,
                column: "creat_time",
                value: new DateTime(2026, 3, 3, 23, 17, 44, 754, DateTimeKind.Local).AddTicks(263));

            migrationBuilder.UpdateData(
                table: "Kullanicilar",
                keyColumn: "Id",
                keyValue: 1,
                column: "creat_time",
                value: new DateTime(2026, 3, 3, 23, 17, 44, 758, DateTimeKind.Local).AddTicks(99));

            migrationBuilder.UpdateData(
                table: "Roller",
                keyColumn: "Id",
                keyValue: 1,
                column: "creat_time",
                value: new DateTime(2026, 3, 3, 23, 17, 44, 757, DateTimeKind.Local).AddTicks(8751));
        }
    }
}
