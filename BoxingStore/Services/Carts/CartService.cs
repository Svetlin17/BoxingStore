namespace BoxingStore.Services.Carts
{
    using BoxingStore.Data;
    using BoxingStore.Data.Models;
    using BoxingStore.Data.Models.Enums;
    using System.Security.Claims;
    using System.Linq;

    public class CartService : ICartService
    {
        private readonly BoxingStoreDbContext data;

        public CartService(BoxingStoreDbContext data)
        {
            this.data = data;
        }

        public Cart GetUserCart(string userId)
        {
            var cartId = this.data.Users.Find(userId).CartId;

            var cart = this.data.Carts.Find(cartId);

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
    }
}
