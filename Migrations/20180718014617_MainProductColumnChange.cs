using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ReactSupply.Migrations
{
    public partial class MainProductColumnChange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "SupplyRecord");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "MainProduct");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "MainProduct");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "MainProduct");

            migrationBuilder.DropColumn(
                name: "ModifiedDate",
                table: "MainProduct");

            migrationBuilder.AddColumn<DateTime>(
                name: "SODate",
                table: "MainProduct",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
               name: "OriginalDate",
               table: "MainProduct",
               nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ActualDate",
                table: "MainProduct",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsOnTime",
                table: "MainProduct",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActualDate",
                table: "MainProduct");

            migrationBuilder.DropColumn(
                name: "IsOnTime",
                table: "MainProduct");

            migrationBuilder.DropColumn(
                name: "OriginalDate",
                table: "MainProduct");

            migrationBuilder.DropColumn(
                name: "SODate",
                table: "MainProduct");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "SupplyRecord",
                unicode: false,
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "MainProduct",
                maxLength: 30,
                nullable: true,
                defaultValueSql: "(suser_name())");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "MainProduct",
                nullable: false,
                defaultValueSql: "(getdate())");

            migrationBuilder.AddColumn<string>(
                name: "ModifiedBy",
                table: "MainProduct",
                maxLength: 30,
                nullable: true,
                defaultValueSql: "(suser_name())");

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDate",
                table: "MainProduct",
                nullable: false,
                defaultValueSql: "(getdate())");
        }
    }
}
