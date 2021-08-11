namespace BoxingStore.Controllers
{
    using BoxingStore.Data;
    using BoxingStore.Data.Models.Enums;
    using BoxingStore.Models;
    using BoxingStore.Models.Cart;
    using BoxingStore.Services.Carts;
    using BoxingStore.Services.Products;
    using Microsoft.AspNetCore.Mvc;
    using System.Collections.Generic;
    using System.Security.Claims;
    using System.Linq;
    using BoxingStore.Data.Models;

    public class CartController : Controller
    {
        private readonly IProductService products;
        private readonly ICartService carts;
        private readonly BoxingStoreDbContext data;

        public CartController(IProductService products, ICartService carts, BoxingStoreDbContext data)
        {
            this.products = products;
            this.carts = carts;
            this.data = data;
        }

        public IActionResult Index() //TODO here I change the size when another person ordered
        {
            var currentCartId = this.data.Users.Find(this.User.FindFirstValue(ClaimTypes.NameIdentifier)).CartId;

            var cart = this.data.Carts.Find(currentCartId);

            var cartProducts = new List<CartProductsQueryModel>();

            foreach (var cartProduct in this.data.CartProducts.Where(x => x.CartId == cart.Id).ToList())
            {
                //If someone else has ordered the quantity is reduced to max quantity------------------
                int maxQuantity = this.products.MaxQuantityAvailable(cartProduct.ProductId, cartProduct.Size);

                if (maxQuantity == 0)
                {
                    this.data.CartProducts.Remove(cartProduct);

                    continue;
                }
                if (cartProduct.Quantity > maxQuantity)
                {
                    cartProduct.Quantity = maxQuantity;
                }
                //-------------------------------------------------------------------------------------

                var product = this.data.Products.Find(cartProduct.ProductId);

                ICollection<ProductSizeQuantity> allSizesForCurrentProduct = this.products.ProductSizeQuantity(product.Id);

                cartProducts.Add(new CartProductsQueryModel
                {
                    Id = cartProduct.Id,
                    Quantity = cartProduct.Quantity,
                    Size = cartProduct.Size,
                    ProductImageUrl = product.ImageUrl,
                    ProductName = product.Brand + " " + product.Name,
                    ProductId = product.Id,
                    Price = product.Price,
                    ProductTotalPrice = product.Price * cartProduct.Quantity, //the totalprice of 1 single product in the cart
                    MaxQuantityAvailable = this.products.MaxQuantityAvailable(product.Id, cartProduct.Size),
                    SizeQuantities = allSizesForCurrentProduct
                });
            }
            this.data.SaveChanges();

            return View(new CartViewModel
            {
                Id = cart.Id,
                CartProducts = cartProducts,
                TotalPrice = this.carts.GetCartTotalPrice(cart.Id)
            });
        }

        public IActionResult EditSize(int id, ProductSize size) //put in cart service
        {
            var cartProduct = this.data.CartProducts.Find(id);

            var currentUserCart = this.carts.GetUserCart(this.User.FindFirstValue(ClaimTypes.NameIdentifier));

            bool cartProductAlreadyExists = this.carts
                                    .IsThisProductWithThisSizeInCart(currentUserCart.Id, cartProduct.ProductId, size);

            if (cartProductAlreadyExists)
            {
                //TODO $"You already have this product with this size in your cart.");
            }
            else
            {
                cartProduct.Size = size;

                this.data.SaveChanges();

                var maxQuantityOfNewSize = this.products.MaxQuantityAvailable(cartProduct.ProductId, cartProduct.Size);

                if (cartProduct.Quantity > maxQuantityOfNewSize)
                {
                    EditQuantity(cartProduct.Id, maxQuantityOfNewSize);
                }
            }

            return RedirectToAction(nameof(Index));
        }

        public IActionResult EditQuantity(int id, int quantity) //put in cart service
        {
            this.data.CartProducts.Find(id).Quantity = quantity;

            this.data.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            var cartProduct = this.data.CartProducts.Find(id);

            if (cartProduct != null)
            {
                this.data.CartProducts.Remove(cartProduct);
                this.data.SaveChanges();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
