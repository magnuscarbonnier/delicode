using DeliCode.Library.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DeliCode.Web.Services
{
    public interface IProductService
    {
        Task<List<Product>> GetAll();
        Task<Product> GetById(Guid id);
    }
}