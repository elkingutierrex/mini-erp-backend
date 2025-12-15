using Microsoft.EntityFrameworkCore;
using MiniErp.Domain.Entities;

namespace MiniErp.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<Permission> Permissions => Set<Permission>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Sale> Sales => Set<Sale>();
    public DbSet<SaleItem> SaleItems => Set<SaleItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // User -> Role (Many to One)
        modelBuilder.Entity<User>()
            .HasOne(u => u.Role)
            .WithMany()
            .IsRequired();

        // Role <-> Permission (Many to Many)
        modelBuilder.Entity<Role>()
            .HasMany(r => r.Permissions)
            .WithMany(p => p.Roles);

        // Sale -> User
        modelBuilder.Entity<Sale>()
            .HasOne(s => s.User)
            .WithMany()
            .IsRequired();

        // Sale -> Items
        modelBuilder.Entity<Sale>()
            .HasMany(s => s.Items)
            .WithOne()
            .IsRequired();

        base.OnModelCreating(modelBuilder);
    }
}
