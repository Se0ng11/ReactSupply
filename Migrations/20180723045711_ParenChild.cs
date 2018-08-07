using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ReactSupply.Migrations
{
    public partial class ParenChild : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "ForeignKey_Menu_SubMenu",
                table: "SubMenu");

            migrationBuilder.DropColumn(
                name: "Parent",
                table: "MainProduct");

            migrationBuilder.CreateTable(
                name: "LineProduct",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Identifier = table.Column<string>(nullable: false),
                    Size = table.Column<string>(nullable: true),
                    Quantity = table.Column<int>(nullable: false),
                    IsRecount = table.Column<bool>(nullable: false),
                    Parent = table.Column<string>(nullable: true),
                    SODate = table.Column<DateTime>(nullable: true),
                    OriginalDate = table.Column<DateTime>(nullable: true),
                    ActualDate = table.Column<DateTime>(nullable: true),
                    IsOnTime = table.Column<bool>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LineProduct", x => x.Identifier);
                    table.ForeignKey(
                        name: "FK_LineProduct_MainProduct_Parent",
                        column: x => x.Parent,
                        principalTable: "MainProduct",
                        principalColumn: "Identifier",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LineProduct_Parent",
                table: "LineProduct",
                column: "Parent");

            migrationBuilder.AddForeignKey(
                name: "FK_SubMenu_Menu_MenuCode",
                table: "SubMenu",
                column: "MenuCode",
                principalTable: "Menu",
                principalColumn: "MenuCode",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubMenu_Menu_MenuCode",
                table: "SubMenu");

            migrationBuilder.DropTable(
                name: "LineProduct");

            migrationBuilder.AddColumn<string>(
                name: "Parent",
                table: "MainProduct",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "ForeignKey_Menu_SubMenu",
                table: "SubMenu",
                column: "MenuCode",
                principalTable: "Menu",
                principalColumn: "MenuCode",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
