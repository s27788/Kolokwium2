using Microsoft.EntityFrameworkCore;
using Kolokwium2.Models;

namespace Kolokwium2.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

    public DbSet<Student> Students => Set<Student>();
    public DbSet<Language> Languages => Set<Language>();
    public DbSet<TaskModel> Tasks => Set<TaskModel>();
    public DbSet<Record> Records => Set<Record>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Student>()
            .HasIndex(s => s.Email)
            .IsUnique();

        modelBuilder.Entity<TaskModel>()
            .HasIndex(t => t.Name)
            .IsUnique();

        base.OnModelCreating(modelBuilder);
    }
}