using Microsoft.EntityFrameworkCore;
using AllTheBeans.Api.Models;

namespace AllTheBeans.Api.Data;

public class AllTheBeansDbContext : DbContext
{
    public AllTheBeansDbContext(DbContextOptions<AllTheBeansDbContext> options)
        : base(options)
    {
    }

    public DbSet<Bean> Beans { get; set; }
    public DbSet<BeanOfTheDay> BeanOfTheDays { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Bean>(entity =>
        {
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Description).HasMaxLength(2000);
            entity.Property(e => e.Country).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Colour).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Image).HasMaxLength(500);
            entity.Property(e => e.ExternalId).HasMaxLength(50);
            entity.Property(e => e.Cost).HasColumnType("decimal(18,2)");
            
            entity.HasIndex(e => e.Name);
            entity.HasIndex(e => e.Country);
            entity.HasIndex(e => e.Colour);
        });

        modelBuilder.Entity<BeanOfTheDay>(entity =>
        {
            entity.HasIndex(e => e.Date).IsUnique();
            
            entity.HasOne(e => e.Bean)
                  .WithMany()
                  .HasForeignKey(e => e.BeanId)
                  .OnDelete(DeleteBehavior.Restrict);
        });
    }
}

