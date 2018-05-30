using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ReactSupply.Migrations
{
    public partial class MenuSubMenu : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "filterRenderer",
                table: "ConfigurationMain",
                newName: "FilterRenderer");

            migrationBuilder.RenameColumn(
                name: "Css",
                table: "ConfigurationMain",
                newName: "HeaderCss");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "SupplyRecord",
                unicode: false,
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BodyCss",
                table: "ConfigurationMain",
                unicode: false,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "JSONResult",
                columns: table => new
                {
                    JsonResult = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JSONResult", x => x.JsonResult);
                });

            migrationBuilder.CreateTable(
                name: "Menu",
                columns: table => new
                {
                    MenuCode = table.Column<string>(maxLength: 20, nullable: false),
                    MenuName = table.Column<string>(maxLength: 50, nullable: true),
                    MenuClass = table.Column<string>(nullable: true),
                    Position = table.Column<decimal>(type: "decimal(4, 2)", nullable: false),
                    Url = table.Column<string>(nullable: true),
                    IsEnabled = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Menu", x => x.MenuCode);
                });

            migrationBuilder.CreateTable(
                name: "SubMenu",
                columns: table => new
                {
                    SubCode = table.Column<string>(maxLength: 20, nullable: false),
                    SubName = table.Column<string>(maxLength: 50, nullable: true),
                    Position = table.Column<decimal>(type: "decimal(4, 2)", nullable: false),
                    SubClass = table.Column<string>(nullable: true),
                    MenuCode = table.Column<string>(maxLength: 20, nullable: false),
                    SubParent = table.Column<string>(nullable: true),
                    Url = table.Column<string>(nullable: true),
                    IsEnabled = table.Column<bool>(nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubMenu", x => x.SubCode);
                    table.ForeignKey(
                        name: "ForeignKey_Menu_SubMenu",
                        column: x => x.MenuCode,
                        principalTable: "Menu",
                        principalColumn: "MenuCode",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SubMenu_MenuCode",
                table: "SubMenu",
                column: "MenuCode");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "JSONResult");

            migrationBuilder.DropTable(
                name: "SubMenu");

            migrationBuilder.DropTable(
                name: "Menu");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "SupplyRecord");

            migrationBuilder.DropColumn(
                name: "BodyCss",
                table: "ConfigurationMain");

            migrationBuilder.RenameColumn(
                name: "FilterRenderer",
                table: "ConfigurationMain",
                newName: "filterRenderer");

            migrationBuilder.RenameColumn(
                name: "HeaderCss",
                table: "ConfigurationMain",
                newName: "Css");
        }
    }
}
