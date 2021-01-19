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

        //TODO needs Revising
        public async Task<bool> UpdateInventoryQuanties(Dictionary<Guid, int> productQuantityValuePairs)
        {
            bool updateSuccessful = CheckIfEnoughInStorage(productQuantityValuePairs);
            if (updateSuccessful == false)
                return updateSuccessful;

            updateSuccessful = ReduceAmountInStorage(productQuantityValuePairs);

            return updateSuccessful;
        }

        //TODO needs revising
        private bool ReduceAmountInStorage(Dictionary<Guid, int> productQuantityValuePairs)
        {
            foreach (var productPair in productQuantityValuePairs)
            {
                var product = _context.Products.SingleOrDefault(p => p.Id == productPair.Key);
                product.AmountInStorage -= productPair.Value;
            }
            _context.SaveChanges();
            return true;
        }

        //TODO Needs revising
        private bool CheckIfEnoughInStorage(Dictionary<Guid, int> products)
        {
            bool amountInStorageIsEnough = true;
            foreach (var productQuantity in products)
            {
                var amountInStorage = _context.Products.FirstOrDefault(p => p.Id == productQuantity.Key).AmountInStorage;

                if (amountInStorage < productQuantity.Value)
                {
                    amountInStorageIsEnough = false;
                    return amountInStorageIsEnough;
                }
                else
                    amountInStorageIsEnough = true;
            }
            return amountInStorageIsEnough;
        }
    }
}
