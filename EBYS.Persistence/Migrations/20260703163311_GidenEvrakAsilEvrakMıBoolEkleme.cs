using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EBYS.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class GidenEvrakAsilEvrakMıBoolEkleme : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsAsilEvrak",
                table: "EvrakEkler",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAsilEvrak",
                table: "EvrakEkler");
        }
    }
}
