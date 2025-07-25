using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KitapApi.Migrations
{
    /// <inheritdoc />
    public partial class AddAciklamaToKategori : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Aciklama",
                table: "Kategoriler",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Aciklama",
                table: "Kategoriler");
        }
    }
}
