﻿using DeliCode.Web.Models;
using DeliCode.Web.Repository;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliCode.Web.Services
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _repository;
        private readonly IProductService _productService;
        private static string _cookieName = "Delicode.CartCookie";
        private readonly CookieOptions _cookieOptions; 

        public CartService(ICartRepository repository,IProductService productService)
        {
            _repository = repository;
            _cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                IsEssential = true,
                SameSite = SameSiteMode.Strict,
                Secure = true
            };
            _productService = productService;
        }
        public async Task<Cart> AddProductToCart(Product product)
        {
            var cart = await GetCart();
            var isProductInCart = await ProductIdExistsInCart(cart, product.Id);
            product = await _productService.Get(product.Id);
            if(isProductInCart && cart.Items.SingleOrDefault(x=>x.Product.Id==product.Id).Quantity < product.AmountInStorage)
            {
                cart.Items.SingleOrDefault(x => x.Product.Id == product.Id).Quantity++;
            }
            else if(isProductInCart && cart.Items.SingleOrDefault(x => x.Product.Id == product.Id).Quantity >= product.AmountInStorage)
            {

            }
            else
            {
                cart.Items.Add(new CartItem { Product = product, Quantity=1 });
            }
            cart=await SaveCart(cart);
            return cart;
        }

        public async Task<Cart> GetCart()
        {
            var sessionId = await GetCartSession();

            var cart = await _repository.GetCart(sessionId);
            var cartitems = new List<CartItem>();
            foreach (var item in cart.Items)
            {
                var product = await _productService.Get(item.Product.Id);
                if (item.Quantity > 0 && item.Quantity <= product.AmountInStorage)
                {
                    cartitems.Add(item);
                }
                
            }
            cart.Items = cartitems;
            return cart;
        }

        private async Task<Guid> GetCartSession()
        {
            var session = await _repository.GetCartCookie(_cookieName);
            if (session == Guid.Empty)
            {
                session = await SetNewCartCookie();
            }
            return session;
        }

        private Task<bool> ProductIdExistsInCart(Cart cart, Guid productId)
        {
            var exists = cart.Items.Exists(x => x.Product.Id == productId);
            return Task.FromResult(exists);
        }

        private async Task<Cart> SaveCart(Cart cart)
        {
            cart = await _repository.SaveCart(cart);
            return cart;
        }

        private async Task<Guid> SetNewCartCookie()
        {
            var session = await _repository.SetCartCookie(Guid.NewGuid(), _cookieOptions, _cookieName);
            return session;
        }
    }
}