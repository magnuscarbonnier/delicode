using DeliCode.ProductAPI.Data;
using DeliCode.ProductAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliCode.ProductAPI.Repository
{
    public class ProductRepository : IProductRepository
    {

        private readonly ProductDbContext _context;
        public ProductRepository(ProductDbContext context)
        {
            _context = context;
        }


        public async Task<Product> AddProduct(Product product)
        {
            try
            {
                _context.Products.Add(product);
                await _context.SaveChangesAsync();
            }
            catch
            {
                product = null;
            }

            return product;
        }


        public async Task<Product> DeleteProduct(Guid Id)
        {
            var order = await _context.Products.FindAsync(Id);

            try
            {
                _context.Products.Remove(order);
                await _context.SaveChangesAsync();
            }
            catch
            {
                order = null;
            }

            return order;
        }

        public async Task<List<Product>> GetAllProducts()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<Product> GetProduct(Guid Id)
        {
            return await _context.Products.SingleOrDefaultAsync(o => o.Id == Id);
        }


        public async Task<Product> UpdateProduct(Product product)
        {
            _context.Entry(product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch
            {
                product = null;
            }

            return product;
        }

        public async Task<bool> ReduceInventoryQuanties(Dictionary<Guid, int> productQuantityValuePairs)
        {
            bool updateSuccessful;
            try
            {
                foreach (var productPair in productQuantityValuePairs)
                {
                    var product = await _context.Products.FindAsync(productPair.Key);
                    product.AmountInStorage -= productPair.Value;
                }
                await _context.SaveChangesAsync();

                updateSuccessful = true;
            }
            catch
            {
                updateSuccessful = false;
            }
            return updateSuccessful;
        }

        public async Task<bool> CheckInventoryQuantities(Dictionary<Guid, int> productsQuantities)
        {
            bool amountInStorageIsEnough = true;
            foreach (var productQuantity in productsQuantities)
            {
                var product = await _context.Products.FindAsync(productQuantity.Key);
                var amountInStorage = product?.AmountInStorage;

                if (amountInStorage >= productQuantity.Value)
                {
                    amountInStorageIsEnough = true;
                }
                else
                { 
                    amountInStorageIsEnough = false;
                    return amountInStorageIsEnough;
                }
            }
            return amountInStorageIsEnough;
        }
    }
}
