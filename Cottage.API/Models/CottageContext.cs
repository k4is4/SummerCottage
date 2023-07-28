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

    public virtual DbSet<CalendarNote> CalendarNotes { get; set; }

    public virtual DbSet<Item> Items { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CalendarNote>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Calendar__3214EC07A3ADB369");

            entity.ToTable("CalendarNote");

            entity.Property(e => e.EndDate).HasColumnType("datetime");
            entity.Property(e => e.Note).HasMaxLength(200);
            entity.Property(e => e.StartDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<Item>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tmp_ms_x__3214EC07266B5D7B");

            entity.ToTable("Item");

            entity.Property(e => e.Comment).HasMaxLength(100);
            entity.Property(e => e.Name).HasMaxLength(30);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
