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
            //get cookie from httpcontext
            var cookie = _httpContextAccessor.HttpContext.Request.Cookies["DeliCode.Web.Cart"];

            //try to check if cookie is of type guid (cartId)
            var result = Guid.TryParse(cookie, out Guid cartId);

            if (!result)
            {
                //if cookie not exists, set new cookie
                return SetCartCookie();
            }
            else
            {
                //return id from cookie
                return cartId.ToString();
            }
        }
        private string SetCartCookie()
        {
            //generate new guid for cart
            var cartId = Guid.NewGuid().ToString();

            //set cookieoptions to secure cookie
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                IsEssential=true,
                SameSite = SameSiteMode.Strict,
                Secure = true
            };
            
            //save cookie to context
            _httpContextAccessor.HttpContext.Response.Cookies.Append("DeliCode.Web.Cart", cartId, cookieOptions);
           
            return cartId;
        }
        
        public Cart GetCart()
        {
            //get cart with actual cartid(found in method getcartcookie()) from session
            var cart = _httpContextAccessor.HttpContext.Session.Get<Cart>(cartId);

            //cart not exists, save new cart to session
            if (cart == null)
            {
                cart = new Cart { SessionId = cartId, Items = new List<CartItem>() };
                SaveCartToSession(cart);
            }

            return cart;
        }

        private void SaveCartToSession(Cart cart)
        {
            //save to session
            _httpContextAccessor.HttpContext.Session.Set<Cart>(cartId, cart);
        }

        public Cart AddProductToCart(Product product)
        {
            var cart = GetCart();

            //check if product already is in cart
            if (cart.Items != null && ProductIdExistsInCart(cart, product.Id))
            {
                //add one extra product of same type
                cart.Items.FirstOrDefault(c => c.Product.Id == product.Id).Quantity++;
            }
            else
            {
                //add one new product to cart
                cart.Items.Add(new CartItem { Product = product, Quantity = 1 });
            }

            //save changes
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
