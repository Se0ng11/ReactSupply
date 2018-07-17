using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ReactSupply.Migrations
{
    public partial class MainProduct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MainProduct",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Identifier = table.Column<string>(nullable: false),
                    Size = table.Column<string>(nullable: true),
                    Quantity = table.Column<int>(nullable: false),
                    IsRecount = table.Column<bool>(nullable: false),
                    Parent = table.Column<string>(nullable: true),
                    ModifiedBy = table.Column<string>(maxLength: 30, nullable: true, defaultValueSql: "(suser_name())"),
                    ModifiedDate = table.Column<DateTime>(nullable: false, defaultValueSql: "(getdate())"),
                    CreatedBy = table.Column<string>(maxLength: 30, nullable: true, defaultValueSql: "(suser_name())"),
                    CreatedDate = table.Column<DateTime>(nullable: false, defaultValueSql: "(getdate())")

                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MainProduct", x => x.Identifier);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MainProduct");
        }
    }
}
