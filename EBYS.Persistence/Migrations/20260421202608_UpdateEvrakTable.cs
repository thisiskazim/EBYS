using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EBYS.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateEvrakTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "EkAdi",
                table: "EvrakEkler",
                newName: "MimeType");

          
            migrationBuilder.AddColumn<string>(
                name: "Ad",
                table: "EvrakEkler",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DosyaUzantisi",
                table: "EvrakEkler",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "DosyaVerisi",
                table: "EvrakEkler",
                type: "bytea",
                nullable: true);


            migrationBuilder.Sql("ALTER TABLE \"Evraklar\" ALTER COLUMN \"EvrakSayisi\" TYPE integer USING (\"EvrakSayisi\"::integer);");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Ad",
                table: "EvrakEkler");

            migrationBuilder.DropColumn(
                name: "DosyaUzantisi",
                table: "EvrakEkler");

            migrationBuilder.DropColumn(
                name: "DosyaVerisi",
                table: "EvrakEkler");

            migrationBuilder.RenameColumn(
                name: "MimeType",
                table: "EvrakEkler",
                newName: "EkAdi");

            migrationBuilder.AlterColumn<string>(
                name: "EvrakSayisi",
                table: "Evraklar",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);
        }
    }
}
