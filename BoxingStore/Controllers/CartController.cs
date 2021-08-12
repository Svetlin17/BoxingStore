namespace BoxingStore.Controllers
{
    using BoxingStore.Data.Models.Enums;
    using BoxingStore.Models.Cart;
    using BoxingStore.Services.Carts;
    using BoxingStore.Services.Products;
    using Microsoft.AspNetCore.Mvc;
    using BoxingStore.Infrastructure;

    public class CartController : Controller
    {
        private readonly IProductService products;
        private readonly ICartService carts;

        public CartController(IProductService products, ICartService carts)
        {
            this.products = products;
            this.carts = carts;
        }

        public IActionResult Index()
        {
            var cart = this.carts.GetUserCart(this.User.Id());

            //if ordered, the quantity is reduced to max quantity
            //if deleted, the product is removed from current user cart
            this.carts.CheckCartProductsForOrderedQuantities(cart.Id); 

            var cartProducts = this.carts.GetCartProductsForCart(cart.Id);

            return View(new CartViewModel
            {
                Id = cart.Id,
                CartProducts = cartProducts,
                TotalPrice = this.carts.GetCartTotalPrice(cart.Id)
            });
        }

        public IActionResult EditSize(int id, ProductSize size) //put in cart service
        {
            var currentUserCart = this.carts.GetUserCart(this.User.Id());

            this.carts.EditCartProductSize(id, size, currentUserCart);

            return RedirectToAction(nameof(Index));
        }

        public IActionResult EditQuantity(int id, int quantity)
        {
            this.carts.EditCartProductQuantity(id, quantity);

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            this.carts.DeleteCartProduct(id);

            return RedirectToAction(nameof(Index));
        }
    }
}
