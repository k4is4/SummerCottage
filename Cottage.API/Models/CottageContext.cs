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

    public virtual DbSet<Item> Items { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Item>(entity =>
        {
            entity.ToTable("Item");
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.Comment)
                .HasMaxLength(100);
            entity.Property(e => e.Name)
                .HasMaxLength(30);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
