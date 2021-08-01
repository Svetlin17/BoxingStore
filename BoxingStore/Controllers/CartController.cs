namespace BoxingStore.Controllers
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

            foreach (var cp in this.data.CartProducts.Where(x => x.CartId == cart.Id).ToList())
            {
                cartProducts.Add(new CartProductsQueryModel
                {
                    Quantity = cp.Quantity,
                    Size = cp.Size
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
