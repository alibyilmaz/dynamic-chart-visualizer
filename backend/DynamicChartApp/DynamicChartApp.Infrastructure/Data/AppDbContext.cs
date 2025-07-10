using DynamicChartApp.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace DynamicChartApp.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<LogEntry> Logs => Set<LogEntry>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Sale> Sales => Set<Sale>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure decimal precision for monetary values
        modelBuilder.Entity<Product>()
            .Property(p => p.Price)
            .HasPrecision(18, 2);

        modelBuilder.Entity<Sale>()
            .Property(s => s.TotalAmount)
            .HasPrecision(18, 2);

        modelBuilder.Entity<Product>().HasData(
            new Product { Id = 1, Name = "Laptop", Category = "Electronics", Price = 1200.00m, Stock = 50 },
            new Product { Id = 2, Name = "Smartphone", Category = "Electronics", Price = 800.00m, Stock = 120 },
            new Product { Id = 3, Name = "Desk Chair", Category = "Furniture", Price = 150.00m, Stock = 75 },
            new Product { Id = 4, Name = "Coffee Maker", Category = "Appliances", Price = 90.00m, Stock = 40 },
            new Product { Id = 5, Name = "Notebook", Category = "Stationery", Price = 5.00m, Stock = 500 },
            new Product { Id = 6, Name = "Pen", Category = "Stationery", Price = 1.50m, Stock = 1000 },
            new Product { Id = 7, Name = "Monitor", Category = "Electronics", Price = 300.00m, Stock = 60 },
            new Product { Id = 8, Name = "Backpack", Category = "Accessories", Price = 45.00m, Stock = 200 },
            new Product { Id = 9, Name = "Water Bottle", Category = "Accessories", Price = 15.00m, Stock = 350 },
            new Product { Id = 10, Name = "Desk Lamp", Category = "Furniture", Price = 35.00m, Stock = 80 }
        );

        modelBuilder.Entity<Sale>().HasData(
            new Sale { Id = 1, ProductId = 1, Quantity = 3, SaleDate = new DateTime(2024, 5, 1), TotalAmount = 3600.00m },
            new Sale { Id = 2, ProductId = 2, Quantity = 5, SaleDate = new DateTime(2024, 5, 2), TotalAmount = 4000.00m },
            new Sale { Id = 3, ProductId = 3, Quantity = 7, SaleDate = new DateTime(2024, 5, 3), TotalAmount = 1050.00m },
            new Sale { Id = 4, ProductId = 4, Quantity = 2, SaleDate = new DateTime(2024, 5, 4), TotalAmount = 180.00m },
            new Sale { Id = 5, ProductId = 5, Quantity = 50, SaleDate = new DateTime(2024, 5, 5), TotalAmount = 250.00m },
            new Sale { Id = 6, ProductId = 6, Quantity = 100, SaleDate = new DateTime(2024, 5, 6), TotalAmount = 150.00m },
            new Sale { Id = 7, ProductId = 7, Quantity = 4, SaleDate = new DateTime(2024, 5, 7), TotalAmount = 1200.00m },
            new Sale { Id = 8, ProductId = 8, Quantity = 10, SaleDate = new DateTime(2024, 5, 8), TotalAmount = 450.00m },
            new Sale { Id = 9, ProductId = 9, Quantity = 20, SaleDate = new DateTime(2024, 5, 9), TotalAmount = 300.00m },
            new Sale { Id = 10, ProductId = 10, Quantity = 6, SaleDate = new DateTime(2024, 5, 10), TotalAmount = 210.00m },
            new Sale { Id = 11, ProductId = 1, Quantity = 2, SaleDate = new DateTime(2024, 5, 11), TotalAmount = 2400.00m },
            new Sale { Id = 12, ProductId = 2, Quantity = 3, SaleDate = new DateTime(2024, 5, 12), TotalAmount = 2400.00m },
            new Sale { Id = 13, ProductId = 3, Quantity = 5, SaleDate = new DateTime(2024, 5, 13), TotalAmount = 750.00m },
            new Sale { Id = 14, ProductId = 4, Quantity = 1, SaleDate = new DateTime(2024, 5, 14), TotalAmount = 90.00m },
            new Sale { Id = 15, ProductId = 5, Quantity = 100, SaleDate = new DateTime(2024, 5, 15), TotalAmount = 500.00m }
        );
    }
}
