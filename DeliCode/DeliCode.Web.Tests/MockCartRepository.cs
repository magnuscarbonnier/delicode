using DeliCode.Web.Models;
using DeliCode.Web.Repository;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliCode.Web.Tests
{
    class MockCartRepository : ICartRepository
    {
        internal Product _product;
        internal List<CartItem> _cartitems;
        internal Cart _cart;
        internal Guid _sessionId = new Guid("c94188c7-156f-49cc-89c6-2adc3108663c");
        public MockCartRepository()
        {
            _product = new Product()
            {
                Id = new Guid("BD8F361A-E5E3-4F33-82CF-2594368D78A9"),
                Name = "Chokladtårta",
                Price = 12.50m,
                Description = "En jättegod tårta"
            };
            _cartitems = new List<CartItem> {
                new CartItem { Product = _product, Quantity=2 },
                new CartItem{Product=new Product{ Id=new Guid("4DF795CF-EA1C-47C1-A4E0-F20742CFE359")}, Quantity=1}
            };
            _cart = new Cart { Items = _cartitems, SessionId = _sessionId };
        }
        public Task<Cart> SaveCart(Cart cart)
        {
            _cart = cart;
            return Task.FromResult(_cart);
        }

        public Task<Cart> GetCart(Guid sessionId)
        {
            var cart = _cart;
            return Task.FromResult(cart);
        }

        public Task<Guid> GetCartCookie(string cookieName)
        {
            return Task.FromResult(_sessionId);
        }

        public Task<Guid> SetCartCookie(Guid id, CookieOptions options, string cookieName)
        {
            _sessionId = id;
            return Task.FromResult(_sessionId);

        }
    }
}
