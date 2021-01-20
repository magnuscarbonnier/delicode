using DeliCode.Web.Models;
using DeliCode.Web.Repository;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace DeliCode.Web.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repository;

        public ProductService(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<Product> Add(Product product)
        {
            return await _repository.Add(product);
        }

        public async Task<Product> Get(Guid id)
        {
            return await _repository.Get(id);
        }

        public async Task<List<Product>> GetAll()
        {
            return await _repository.GetAll();
        }

        public async Task<Product> Remove(Guid id)
        {
            return await _repository.Remove(id);
        }

        public async Task<Product> Update(Product product)
        {
            return await _repository.Update(product);
        }

        public async Task<bool> UpdateInventoryAmount(List<OrderProduct> orderProducts)
        {

            var productsKeyValuePairs = MapOrderProductsToDictionary(orderProducts);

            return await _repository.UpdateInventoryAmount(productsKeyValuePairs);
        }

        private Dictionary<Guid, int> MapOrderProductsToDictionary(List<OrderProduct> orderProducts)
        {
            Dictionary<Guid, int> productQuantityValuePairs = new Dictionary<Guid, int>();
            foreach (var product in orderProducts)
            {
                productQuantityValuePairs.Add(product.Id, product.Quantity);
            }

            return productQuantityValuePairs;
        }
    }
}
