namespace BoxingStore.Services.Carts
{
    using BoxingStore.Data.Models;
    using BoxingStore.Data.Models.Enums;

    public interface ICartService
    {
        Cart GetUserCart(string userId);

        public double GetCartTotalPrice(int cartId);
        bool IsThisProductWithThisSizeInCart(int cartId, int productId, ProductSize size);
    }
}
