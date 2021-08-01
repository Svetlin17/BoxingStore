﻿namespace BoxingStore.Controllers
{
    using BoxingStore.Data;
    using BoxingStore.Models;
    using BoxingStore.Models.Cart;
    using Microsoft.AspNetCore.Mvc;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    public class CartController : Controller
    {
        private readonly BoxingStoreDbContext data;

        public CartController(BoxingStoreDbContext data)
        {
            this.data = data;
        }

        public IActionResult Cart()
        {
            var cart = this.data.Carts.Where(x => x.Id == 2).FirstOrDefault(); //TODO CurrentUser.Cart

            var cartProducts = new List<CartProductsQueryModel>();

            foreach (var cartProduct in this.data.CartProducts.Where(x => x.CartId == cart.Id).ToList())
            {
                var product = this.data.Products.Find(cartProduct.ProductId);

                cartProducts.Add(new CartProductsQueryModel
                {
                    Quantity = cartProduct.Quantity,
                    Size = cartProduct.Size,
                    ProductImageUrl = product.ImageUrl,
                    ProductName = product.Brand + " " + product.Name,
                    ProductConvertedName = product.ConvertedName
                });
            }

            return this.View(new CartViewModel
            {
                Id = cart.Id,
                CartProducts = cartProducts
            });
        }
    }
}