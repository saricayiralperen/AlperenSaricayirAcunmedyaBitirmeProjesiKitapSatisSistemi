using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KitapApi.Migrations
{
    /// <inheritdoc />
    public partial class UpdateKullaniciFieldsForAuth : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Ad",
                table: "Kullanicilar");

            migrationBuilder.DropColumn(
                name: "Eposta",
                table: "Kullanicilar");

            migrationBuilder.DropColumn(
                name: "Sifre",
                table: "Kullanicilar");

            migrationBuilder.DropColumn(
                name: "Soyad",
                table: "Kullanicilar");

            migrationBuilder.AlterColumn<string>(
                name: "Rol",
                table: "Kullanicilar",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "AdSoyad",
                table: "Kullanicilar",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Kullanicilar",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "KayitTarihi",
                table: "Kullanicilar",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "SifreHash",
                table: "Kullanicilar",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdSoyad",
                table: "Kullanicilar");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Kullanicilar");

            migrationBuilder.DropColumn(
                name: "KayitTarihi",
                table: "Kullanicilar");

            migrationBuilder.DropColumn(
                name: "SifreHash",
                table: "Kullanicilar");

            migrationBuilder.AlterColumn<string>(
                name: "Rol",
                table: "Kullanicilar",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AddColumn<string>(
                name: "Ad",
                table: "Kullanicilar",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Eposta",
                table: "Kullanicilar",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Sifre",
                table: "Kullanicilar",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Soyad",
                table: "Kullanicilar",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
