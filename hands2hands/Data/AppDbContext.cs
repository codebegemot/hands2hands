using System;
using hands2hands.Models;
using Microsoft.EntityFrameworkCore;

namespace hands2hands.DTOs;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Item> Items => Set<Item>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<ItemCategory> ItemCategories => Set<ItemCategory>();
    public DbSet<Media> Media => Set<Media>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Item>()
                   .Property(i => i.Id)
                   .HasDefaultValueSql("gen_random_uuid()");

        modelBuilder.Entity<ItemCategory>()
                .HasKey(ic => new { ic.ItemId, ic.CategoryId });

        modelBuilder.Entity<ItemCategory>()
                .HasOne(ic => ic.Item)
                .WithMany(i => i.ItemCategories)
                .HasForeignKey(ic => ic.ItemId);

        modelBuilder.Entity<ItemCategory>()
                .HasOne(ic => ic.Category)
                .WithMany(c => c.ItemCategories)
                .HasForeignKey(ic => ic.CategoryId);

        modelBuilder.Entity<Media>()
                .HasOne(m => m.Item)
                .WithMany(i => i.Media)
                .HasForeignKey(m => m.ItemId)
                .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Item>()
                .Property(i => i.Price)
                .HasColumnType("decimal(18,2)");
    }
}
