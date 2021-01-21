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
            base.Database.Migrate();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Product>().HasData
            (
                new Product() { Id = Guid.NewGuid(), Name = "Kanelbulle", Description = "Varma, frysta", Price = 9.50m, ImageUrl = "img/bullar-min.jpg", AmountInStorage = 3 },
                new Product() { Id = Guid.NewGuid(), Name = "Kladdkaka", Description = "Extra florsocker, extra kall", Price = 50.00m, ImageUrl = "img/kladdkaka-min.jpg", AmountInStorage = 0 },
                new Product() { Id = Guid.NewGuid(), Name = "Tårta", Description = "Innehåller grädde och is", Price = 79.90m, ImageUrl = "img/tarta-min.jpg", AmountInStorage = 3 },
                new Product() { Id = Guid.NewGuid(), Name = "Cheesecake", Description = "En vanlig cheesecake, mellanfryst", Price = 29.90m, ImageUrl = "img/ostkaka-min.jpg", AmountInStorage = 1 },
                new Product() { Id = Guid.NewGuid(), Name = "Muffin", Description = "Stora, tinade", Price = 19.90m, ImageUrl = "img/muffin-min.jpg", AmountInStorage = 4 }
            );
        }
    }
}