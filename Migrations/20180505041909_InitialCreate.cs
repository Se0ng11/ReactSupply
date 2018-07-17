using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ReactSupply.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ConfigurationMain",  
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ModuleID = table.Column<int>(nullable: false, defaultValueSql: "((0))"),
                    ValueName = table.Column<string>(maxLength: 50, nullable: false),
                    DisplayName = table.Column<string>(maxLength: 100, nullable: true),
                    ControlType = table.Column<string>(unicode: false, maxLength: 10, nullable: true, defaultValueSql: "('text')"),
                    DefaultText = table.Column<string>(maxLength: 1000, nullable: true),
                    Description = table.Column<string>(maxLength: 1000, nullable: true),
                    MaxLength = table.Column<int>(nullable: true, defaultValueSql: "((2000))"),
                    MinLength = table.Column<int>(nullable: true, defaultValueSql: "((0))"),
                    Position = table.Column<decimal>(type: "decimal(4, 2)", nullable: true),
                    Width = table.Column<int>(nullable: true, defaultValueSql: "((6))"),
                    Group = table.Column<string>(maxLength: 100, nullable: true),
                    IsEditable = table.Column<bool>(nullable: false),
                    IsFilterable = table.Column<bool>(nullable: false),
                    IsLocked = table.Column<bool>(nullable: false),
                    IsRequired = table.Column<bool>(nullable: false),
                    IsResizeable = table.Column<bool>(nullable: false),
                    IsSortable = table.Column<bool>(nullable: false),
                    IsVisible = table.Column<bool>(nullable: false),
                    Css = table.Column<string>(unicode: false, nullable: true),
                    Editor = table.Column<string>(unicode: false, maxLength: 100, nullable: true),
                    filterRenderer = table.Column<string>(unicode: false, maxLength: 100, nullable: true),
                    Formatter = table.Column<string>(unicode: false, maxLength: 100, nullable: true),
                    HeaderRenderer = table.Column<string>(unicode: false, maxLength: 100, nullable: true),
                   
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConfigurationMain", x => new { x.ModuleID, x.ValueName });
                });

            migrationBuilder.CreateTable(
                name: "SupplyRecord",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ModuleID = table.Column<int>(nullable: false, defaultValueSql: "((0))"),
                    ValueName = table.Column<string>(maxLength: 50, nullable: false),
                    AxNumber = table.Column<string>(maxLength: 20, nullable: false),
                    Data = table.Column<string>(maxLength: 1000, nullable: true),
                    ModifiedBy = table.Column<string>(maxLength: 30, nullable: true, defaultValueSql: "(suser_name())"),
                    ModifiedDate = table.Column<DateTime>(nullable: true, defaultValueSql: "(getdate())"),
                    CreatedBy = table.Column<string>(maxLength: 30, nullable: true, defaultValueSql: "(suser_name())"),
                    CreatedDate = table.Column<DateTime>(nullable: true, defaultValueSql: "(getdate())")
                  
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupplyRecord", x => new { x.Id, x.ModuleID, x.ValueName });
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConfigurationMain");

            migrationBuilder.DropTable(
                name: "SupplyRecord");
        }
    }
}
