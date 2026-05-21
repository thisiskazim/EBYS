using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EBYS.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateGelenEvrakSevk1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GelenEvrakDurumEnum",
                table: "GelenEvrakSevkler",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GelenEvrakDurumEnum",
                table: "GelenEvrakSevkler");
        }
    }
}
