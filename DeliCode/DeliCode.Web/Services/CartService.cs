using DeliCode.Web.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliCode.Web.Services
{
    public interface ICartService
    {
        Cart GetCart();
        Cart AddProductToCart(Product product);
    }
    public class CartService : ICartService
    {
        
        private readonly IHttpContextAccessor _httpContextAccessor;
        private string cartId;
        public CartService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            cartId = GetCartCookie();
        }
        private string GetCartCookie()
        {
            var cookie = _httpContextAccessor.HttpContext.Request.Cookies["DeliCode.Web.Cart"];
            var result = Guid.TryParse(cookie, out Guid cartId);
            if (!result)
            {
                return SetCartCookie();
            }
            else
            {
                return cartId.ToString();
            }
        }
        private string SetCartCookie()
        {
            var cartId = Guid.NewGuid().ToString();
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                SameSite = SameSiteMode.Strict,
                Secure = true
            };
            
            _httpContextAccessor.HttpContext.Response.Cookies.Append("DeliCode.Web.Cart", cartId, cookieOptions);
           
            return cartId;
        }
        
        public Cart GetCart()
        {
            var cart = _httpContextAccessor.HttpContext.Session.Get<Cart>(cartId);
            if (cart == null)
            {
                cart = new Cart { SessionId = cartId, Items = new List<CartItem>() };
                SaveCartToSession(cart);
            }

            return cart;
        }

        private void SaveCartToSession(Cart cart)
        {
            _httpContextAccessor.HttpContext.Session.Set<Cart>(cartId, cart);
        }

        public Cart AddProductToCart(Product product)
        {
            var cart = GetCart();

            if (cart.Items != null && ProductIdExistsInCart(cart, product.Id))
            {
                cart.Items.FirstOrDefault(c => c.Product.Id == product.Id).Quantity++;
            }
            else
            {
                cart.Items.Add(new CartItem { Product = product, Quantity = 1 });
            }

            SaveCartToSession(cart);

            return cart;
        }

        private bool ProductIdExistsInCart(Cart cart, Guid productId)
        {
            var exists = false;
            var product = cart.Items.FirstOrDefault(c => c.Product.Id == productId);
            if (product != null)
            {
                exists = true;
            }
            return exists;
        }
        
    }
}
