namespace BoxingStore.Services.Carts
{
    using BoxingStore.Data.Models;

    public interface ICartService
    {
        Cart GetUserCart(string userId);

        public double GetCartTotalPrice(int cartId);
    }
}
