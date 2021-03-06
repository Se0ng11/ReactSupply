﻿using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ReactSupply.Models.DB
{
    public partial class SupplyChainContext : DbContext
    {
        public virtual DbSet<ConfigurationMain> ConfigurationMain { get; set; }
        public virtual DbSet<SupplyRecord> SupplyRecord { get; set; }
        public virtual DbSet<Menu> Menu { get; set; }
        public virtual DbSet<SubMenu> SubMenu { get; set; }
        public virtual DbSet<History> History { get; set; }
        public virtual DbSet<Setting> Setting { get; set; }
        public virtual DbSet<Tracker> Tracker { get; set; }
        public virtual DbSet<MainProduct> MainProduct { get; set; }
        public virtual DbSet<LineProduct> LineProduct { get; set; }
        public virtual DbSet<RolesMenu> RolesMenu { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ConfigurationMain>(entity =>
            {
                entity.HasKey(e => new { e.ModuleId, e.ValueName });

                entity.Property(e => e.ModuleId)
                    .HasColumnName("ModuleID")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.ValueName).HasMaxLength(50);

                entity.Property(e => e.BodyCss).IsUnicode(false);

                entity.Property(e => e.ControlType)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('text')");

                entity.Property(e => e.DefaultText).HasMaxLength(1000);

                entity.Property(e => e.Description).HasMaxLength(1000);

                entity.Property(e => e.DisplayName).HasMaxLength(100);

                entity.Property(e => e.Editor)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.FilterRenderer)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Formatter)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Group).HasMaxLength(100);

                entity.Property(e => e.HeaderCss).IsUnicode(false);

                entity.Property(e => e.HeaderRenderer)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Id).ValueGeneratedOnAdd().Metadata.AfterSaveBehavior = PropertySaveBehavior.Ignore;

                entity.Property(e => e.MaxLength).HasDefaultValueSql("((2000))");

                entity.Property(e => e.MinLength).HasDefaultValueSql("((0))");

                entity.Property(e => e.Position).HasColumnType("decimal(4, 2)");

                entity.Property(e => e.Width).HasDefaultValueSql("((6))");
            });

            modelBuilder.Entity<SupplyRecord>(entity =>
            {
                entity.HasKey(e => new { e.Id, e.ModuleId, e.ValueName });

                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.ModuleId)
                    .HasColumnName("ModuleID")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.ValueName).HasMaxLength(50);

                entity.Property(e => e.Identifier)
                    .IsRequired();

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(30)
                    .HasDefaultValueSql("(suser_name())");

                entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Data).HasMaxLength(1000);

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(30)
                    .HasDefaultValueSql("(suser_name())");

                entity.Property(e => e.ModifiedDate).HasDefaultValueSql("(getdate())");
            });


            modelBuilder.Entity<Menu>(entity =>
            {
                entity.HasKey(e => new { e.MenuCode });

                entity.Property(e => e.MenuCode).HasMaxLength(20).IsRequired();

                entity.Property(e => e.MenuName).HasMaxLength(50);

                entity.Property(e => e.Position).HasColumnType("decimal(4, 2)");
            });

            modelBuilder.Entity<SubMenu>(entity =>
            {
                entity.HasKey(e => new { e.SubCode });

                entity.Property(e => e.SubCode).HasMaxLength(20).IsRequired();

                entity.Property(e => e.SubName).HasMaxLength(50);

                entity.Property(e => e.MenuCode).HasMaxLength(20).IsRequired();

                entity.Property(e => e.Position).HasColumnType("decimal(4, 2)");
            });

            modelBuilder.Entity<History>(entity =>
            {
                entity.HasKey(e => new { e.Id, e.ModuleId, e.Identifier });
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<Setting>(entity =>
            {
                entity.HasKey(e => new { e.Code });
                entity.Property(e => e.Code).HasMaxLength(20);
            });

            modelBuilder.Entity<Tracker>(entity =>
            {
                entity.HasKey(e => new { e.AffectField });
            });

            modelBuilder.Entity<MainProduct>(entity =>
            {
                entity.HasKey(e => new { e.Identifier });
                entity.Property(e => e.Id).ValueGeneratedOnAdd().Metadata.AfterSaveBehavior = PropertySaveBehavior.Ignore;
            });

            modelBuilder.Entity<MainProduct>().HasMany(b => b.LineProduct).WithOne(p => p.MainProduct)
            .HasForeignKey(p => p.Parent)
            .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<LineProduct>(entity =>
            {
                entity.HasKey(e => new { e.Identifier });
                entity.Property(e => e.Id).ValueGeneratedOnAdd().Metadata.AfterSaveBehavior = PropertySaveBehavior.Ignore;
            });

            modelBuilder.Entity<RolesMenu>(entity =>
            {
                entity.HasKey(e => new { e.RolesId, e.MenuId });
            });
        }
    }
}
