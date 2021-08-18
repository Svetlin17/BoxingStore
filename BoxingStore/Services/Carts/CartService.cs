namespace BoxingStore.Services.Carts
{
    using BoxingStore.Data;
    using BoxingStore.Data.Models;
    using BoxingStore.Data.Models.Enums;
    using BoxingStore.Models.Cart;
    using BoxingStore.Models;
    using BoxingStore.Services.Products;
    using System.Collections.Generic;
    using System.Linq;

    public class CartService : ICartService
    {
        private readonly IProductService products;
        private readonly BoxingStoreDbContext data;

        public CartService(IProductService products, BoxingStoreDbContext data)
        {
            this.products = products;
            this.data = data;
        }

        public CartViewModel GetUserCart(string userId)
        {
            var cartId = this.data.Users.Find(userId).CartId;

            var cartProducts = this.data.CartProducts.Where(x => x.CartId == cartId).ToList();

            var cart = this.data
                .Carts
                .Where(c => c.Id == cartId)
                .Select(c => new CartViewModel
                {
                    Id = c.Id
                })
                .FirstOrDefault();

            return cart;
        }

        public double GetCartTotalPrice(int cartId)
        {
            var cartTotalPrice = 0.0;

            foreach (var cartProduct in this.data.CartProducts.Where(x => x.CartId == cartId).ToList())
            {
                var product = this.data.Products.Find(cartProduct.ProductId);

                cartTotalPrice += product.Price * cartProduct.Quantity;
            }

            return cartTotalPrice;
        }

        public bool IsThisProductWithThisSizeInCart(int cartId, int productId, ProductSize size)
        {
            return this.data.CartProducts.Any(cp => cp.CartId == cartId && cp.ProductId == productId && cp.Size == size);
        }

        public IEnumerable<CartProductsQueryModel> GetCartProductsForCart(int cartId)
        {
            var cartProducts = new List<CartProductsQueryModel>();

            foreach (var cartProduct in this.data.CartProducts.Where(x => x.CartId == cartId).ToList())
            {
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
                    MaxQuantityAvailable = this.products.MaxQuantityOfSizeAvailable(product.Id, cartProduct.Size),
                    SizeQuantities = allSizesForCurrentProduct
                });
            }

            return cartProducts;
        }

        public void CheckCartProductsForOrderedQuantities(int cartId)
        {
            var cartProducts = this.data.CartProducts.Where(x => x.CartId == cartId).ToList();

            foreach (var cartProduct in cartProducts)
            {
                int maxQuantity = this.products.MaxQuantityOfSizeAvailable(cartProduct.ProductId, cartProduct.Size);

                if (maxQuantity == 0)
                {
                    this.data.CartProducts.Remove(cartProduct); //remove for the current user only

                    continue;
                }
                if (cartProduct.Quantity > maxQuantity) //if someone else has ordered, the quantity is reduced to max quantity
                {
                    cartProduct.Quantity = maxQuantity;
                }
            }
            this.data.SaveChanges();
        }

        public void EditCartProductQuantity(int id, int quantity)
        {
            this.data.CartProducts.Find(id).Quantity = quantity;

            this.data.SaveChanges();
        }

        public void EditCartProductSize(int id, ProductSize size, CartViewModel currentUserCart)
        {
            var cartProduct = this.data.CartProducts.Find(id);

            bool cartProductAlreadyExists = IsThisProductWithThisSizeInCart(currentUserCart.Id, cartProduct.ProductId, size);

            if (cartProductAlreadyExists)
            {
                //TODO $"You already have this product with this size in your cart.");
            }
            else
            {
                cartProduct.Size = size;

                this.data.SaveChanges();

                var maxQuantityOfNewSize = this.products.MaxQuantityOfSizeAvailable(cartProduct.ProductId, cartProduct.Size);

                if (cartProduct.Quantity > maxQuantityOfNewSize)
                {
                    EditCartProductQuantity(cartProduct.Id, maxQuantityOfNewSize);
                }
            }

        }

        public void DeleteCartProduct(int cartProductId)
        {
            var cartProduct = this.data.CartProducts.Find(cartProductId);

            if (cartProduct != null)
            {
                this.data.CartProducts.Remove(cartProduct);
                this.data.SaveChanges();
            }
        }
    }
}
