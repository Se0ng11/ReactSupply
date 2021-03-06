﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using ReactSupply.Models.DB;
using System;

namespace ReactSupply.Migrations
{
    [DbContext(typeof(SupplyChainContext))]
    [Migration("20180614033838_History")]
    partial class History
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.3-rtm-10026")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("ReactSupply.Models.DB.ConfigurationMain", b =>
                {
                    b.Property<int>("ModuleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("ModuleID")
                        .HasDefaultValueSql("((0))");

                    b.Property<string>("ValueName")
                        .HasMaxLength(50);

                    b.Property<string>("BodyCss")
                        .IsUnicode(false);

                    b.Property<string>("ControlType")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("('text')")
                        .HasMaxLength(10)
                        .IsUnicode(false);

                    b.Property<string>("DefaultText")
                        .HasMaxLength(1000);

                    b.Property<string>("Description")
                        .HasMaxLength(1000);

                    b.Property<string>("DisplayName")
                        .HasMaxLength(100);

                    b.Property<string>("Editor")
                        .HasMaxLength(100)
                        .IsUnicode(false);

                    b.Property<string>("FilterRenderer")
                        .HasMaxLength(100)
                        .IsUnicode(false);

                    b.Property<string>("Formatter")
                        .HasMaxLength(100)
                        .IsUnicode(false);

                    b.Property<string>("Group")
                        .HasMaxLength(100);

                    b.Property<string>("HeaderCss")
                        .IsUnicode(false);

                    b.Property<string>("HeaderRenderer")
                        .HasMaxLength(100)
                        .IsUnicode(false);

                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("IsEditable");

                    b.Property<bool>("IsFilterable");

                    b.Property<bool>("IsLocked");

                    b.Property<bool>("IsRequired");

                    b.Property<bool>("IsResizeable");

                    b.Property<bool>("IsSortable");

                    b.Property<bool>("IsVisible");

                    b.Property<int?>("MaxLength")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("((2000))");

                    b.Property<int?>("MinLength")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("((0))");

                    b.Property<decimal?>("Position")
                        .HasColumnType("decimal(4, 2)");

                    b.Property<int?>("Width")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("((6))");

                    b.HasKey("ModuleId", "ValueName");

                    b.ToTable("ConfigurationMain");
                });

            modelBuilder.Entity("ReactSupply.Models.DB.History", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ModuleId");

                    b.Property<string>("Identifier");

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("CreatedDate");

                    b.Property<string>("Message");

                    b.HasKey("Id", "ModuleId", "Identifier");

                    b.ToTable("History");
                });

            modelBuilder.Entity("ReactSupply.Models.DB.Menu", b =>
                {
                    b.Property<string>("MenuCode")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(20);

                    b.Property<bool>("IsEnabled");

                    b.Property<string>("MenuClass");

                    b.Property<string>("MenuName")
                        .HasMaxLength(50);

                    b.Property<decimal>("Position")
                        .HasColumnType("decimal(4, 2)");

                    b.Property<string>("Url");

                    b.HasKey("MenuCode");

                    b.ToTable("Menu");
                });

            modelBuilder.Entity("ReactSupply.Models.DB.SubMenu", b =>
                {
                    b.Property<string>("SubCode")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(20);

                    b.Property<bool>("IsEnabled");

                    b.Property<string>("MenuCode")
                        .IsRequired()
                        .HasMaxLength(20);

                    b.Property<decimal>("Position")
                        .HasColumnType("decimal(4, 2)");

                    b.Property<string>("SubClass");

                    b.Property<string>("SubName")
                        .HasMaxLength(50);

                    b.Property<string>("SubParent");

                    b.Property<string>("Url");

                    b.HasKey("SubCode");

                    b.HasIndex("MenuCode");

                    b.ToTable("SubMenu");
                });

            modelBuilder.Entity("ReactSupply.Models.DB.SupplyRecord", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ModuleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("ModuleID")
                        .HasDefaultValueSql("((0))");

                    b.Property<string>("ValueName")
                        .HasMaxLength(50);

                    b.Property<string>("Identifier")
                        .IsRequired()
                        .HasMaxLength(20);

                    b.Property<string>("CreatedBy")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("(suser_name())")
                        .HasMaxLength(30);

                    b.Property<DateTime?>("CreatedDate")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("(getdate())");

                    b.Property<string>("Data")
                        .HasMaxLength(1000);

                    b.Property<string>("ModifiedBy")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("(suser_name())")
                        .HasMaxLength(30);

                    b.Property<DateTime?>("ModifiedDate")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("(getdate())");

                    b.Property<string>("Status")
                        .HasMaxLength(10)
                        .IsUnicode(false);

                    b.HasKey("Id", "ModuleId", "ValueName");

                    b.ToTable("SupplyRecord");
                });

            modelBuilder.Entity("ReactSupply.Models.Entity.JSONResult", b =>
                {
                    b.Property<string>("JsonResult")
                        .ValueGeneratedOnAdd();

                    b.HasKey("JsonResult");

                    b.ToTable("JSONResult");
                });

            modelBuilder.Entity("ReactSupply.Models.DB.SubMenu", b =>
                {
                    b.HasOne("ReactSupply.Models.DB.Menu", "Menu")
                        .WithMany("SubMenus")
                        .HasForeignKey("MenuCode")
                        .HasConstraintName("ForeignKey_Menu_SubMenu")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
