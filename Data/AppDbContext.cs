using BlogSeoAi.Models;
using Microsoft.EntityFrameworkCore;

namespace BlogSeoAi.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Post> Posts => Set<Post>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Post>()
            .HasIndex(p => p.Slug)
            .IsUnique();

        modelBuilder.Entity<Post>()
            .HasIndex(p => new { p.Status, p.PublishedAtUtc });

        base.OnModelCreating(modelBuilder);
    }
}