using Demo.Models;
using Microsoft.EntityFrameworkCore;

namespace Demo.Repository.Database;

public sealed class ShopContext : DbContext
{
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Order> Orders { get; set; }

    public ShopContext()
    {
        Database.EnsureDeleted();
        Database.EnsureCreated();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder
                .UseLazyLoadingProxies()
                .UseInMemoryDatabase("shopdatabase");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(customer => customer.CustomerId);
            entity.Property(customer => customer.Name).IsRequired();
            entity.Property(customer => customer.Email).IsRequired();
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(order => order.OrderId);
            entity.Property(order => order.TotalAmount).HasPrecision(18, 2);
            entity.HasOne(order => order.Customer)
                .WithMany(customer => customer.Orders)
                .HasForeignKey(order => order.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Customer>().HasData(
            new Customer { CustomerId = 1, Name = "Kovács Anna", Email = "anna@example.com" },
            new Customer { CustomerId = 2, Name = "Nagy Béla", Email = "bela@example.com" },
            new Customer { CustomerId = 3, Name = "Szabó Csilla", Email = "csilla@example.com" });

        modelBuilder.Entity<Order>().HasData(
            new Order { OrderId = 1, CustomerId = 1, OrderDate = new DateTime(2026, 9, 1), TotalAmount = 12500m, IsPaid = true },
            new Order { OrderId = 2, CustomerId = 1, OrderDate = new DateTime(2026, 9, 8), TotalAmount = 34900m, IsPaid = false },
            new Order { OrderId = 3, CustomerId = 2, OrderDate = new DateTime(2026, 9, 3), TotalAmount = 8900m, IsPaid = true },
            new Order { OrderId = 4, CustomerId = 2, OrderDate = new DateTime(2026, 9, 12), TotalAmount = 59900m, IsPaid = false },
            new Order { OrderId = 5, CustomerId = 3, OrderDate = new DateTime(2026, 9, 5), TotalAmount = 21900m, IsPaid = true });
    }
}
