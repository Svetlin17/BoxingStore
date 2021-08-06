namespace BoxingStore.Controllers
{
    using BoxingStore.Data;
    using BoxingStore.Models;
    using BoxingStore.Models.Cart;
    using Microsoft.AspNetCore.Mvc;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;

    public class CartController : Controller
    {
        private readonly BoxingStoreDbContext data;

        public CartController(BoxingStoreDbContext data)
        {
            this.data = data;
        }

        public IActionResult Index()
        {
            var currentCartId = this.data.Users.Find(this.User.FindFirstValue(ClaimTypes.NameIdentifier)).CartId;

            var cart = this.data.Carts.Find(currentCartId);

            var cartProducts = new List<CartProductsQueryModel>();

            var totalPrice = 0.0;

            foreach (var cartProduct in this.data.CartProducts.Where(x => x.CartId == cart.Id).ToList())
            {
                var product = this.data.Products.Find(cartProduct.ProductId); //why not through cartProduct.Product ?

                cartProducts.Add(new CartProductsQueryModel
                {
                    Id = cartProduct.Id,
                    Quantity = cartProduct.Quantity,
                    Size = cartProduct.Size,
                    ProductImageUrl = product.ImageUrl,  //cartProduct.Product.ImageUrl,
                    ProductName = product.Brand + " " + product.Name,
                    ProductId = product.Id,
                    Price = product.Price,
                    TotalPrice = product.Price * cartProduct.Quantity
                });

                totalPrice += product.Price * cartProduct.Quantity;
            }

            return View(new CartViewModel
            {
                Id = cart.Id,
                CartProducts = cartProducts,
                TotalPrice = totalPrice
            });
        }

        public IActionResult Edit(int id, int quantity)
        {
            this.data.CartProducts.Find(id).Quantity = quantity;

            this.data.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            var cartProduct = this.data.CartProducts.Find(id);

            this.data.CartProducts.Remove(cartProduct);

            this.data.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
    }
}
