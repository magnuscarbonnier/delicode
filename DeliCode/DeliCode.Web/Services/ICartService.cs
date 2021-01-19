using DeliCode.Web.Models;
using System;
using System.Threading.Tasks;

namespace DeliCode.Web.Services
{
    public interface ICartService
    {
        Task<Cart> GetCart();
        Task<Cart> AddProductToCart(Guid id);
    }
}
