using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DeliCode.ProductAPI.Models;

namespace DeliCode.ProductAPI.Data
{
    public class ProductDbContext : DbContext
    {
        public DbSet<Product> Products { get; set; }

        public ProductDbContext(DbContextOptions<ProductDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Product>().HasData
            (
                new Product() { Id = Guid.NewGuid(), Name = "Kanelbulle", Description = "Varma", Price = 9.50m, ImageUrl = "https://picsum.photos/id/133/286/180", AmountInStorage = 3 },
                new Product() { Id = Guid.NewGuid(), Name = "Kladdkarta", Description = "Extra florsocker", Price = 50.00m, ImageUrl = "https://picsum.photos/id/106/286/180", AmountInStorage = 0 },
                new Product() { Id = Guid.NewGuid(), Name = "Tårta", Description = "Innehåller grädde", Price = 79.90m, ImageUrl = "https://picsum.photos/id/292/286/180", AmountInStorage = 3 },
                new Product() { Id = Guid.NewGuid(), Name = "Cheesecake", Description = "En vanlig cheesecake", Price = 29.90m, ImageUrl = "https://picsum.photos/id/104/286/180", AmountInStorage = 1 },
                new Product() { Id = Guid.NewGuid(), Name = "Muffin", Description = "Stora", Price = 19.90m, ImageUrl = "https://picsum.photos/id/143/286/180", AmountInStorage = 4 }
            );
        }
    }
}
