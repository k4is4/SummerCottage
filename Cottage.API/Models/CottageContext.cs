using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Cottage.API.Models;

public partial class CottageContext : DbContext
{
    public CottageContext()
    {
    }

    public CottageContext(DbContextOptions<CottageContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Item> Items { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Category__3214EC070705EDFB");

            entity.ToTable("Category");

            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.Name)
                .HasMaxLength(20);
        });

        modelBuilder.Entity<Item>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Item__3214EC074F34235D");

            entity.ToTable("Item");

            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.Comment)
                .HasMaxLength(300);
            entity.Property(e => e.Name)
                .HasMaxLength(50);

            entity.HasOne(d => d.CategoryNavigation).WithMany(p => p.Items)
                .HasForeignKey(d => d.Category)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Item__Category__267ABA7A");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
