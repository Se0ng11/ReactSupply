using Microsoft.EntityFrameworkCore.Migrations;

namespace ReactSupply.Migrations
{
    public partial class RolesMenu : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RolesMenu",
                columns: table => new
                {
                    RolesId = table.Column<string>(nullable: false),
                    MenuId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolesMenu", x => new { x.RolesId, x.MenuId });
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RolesMenu");
        }
    }
}
