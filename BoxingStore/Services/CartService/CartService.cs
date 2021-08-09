namespace BoxingStore.Services.CartService
{
    using AutoMapper;
    using BoxingStore.Data;
    using BoxingStore.Data.Models;
    using System.Linq;

    public class CartService : ICartService
    {
        private readonly BoxingStoreDbContext data;
        private readonly IConfigurationProvider mapper;

        public CartService(BoxingStoreDbContext data, IMapper mapper)
        {
            this.data = data;
            this.mapper = mapper.ConfigurationProvider;
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
    }
}
