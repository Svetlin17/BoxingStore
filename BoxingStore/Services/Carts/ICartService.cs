namespace BoxingStore.Services.Carts
{
    using BoxingStore.Data.Models.Enums;
    using BoxingStore.Models;
    using BoxingStore.Models.Cart;
    using System.Collections.Generic;

    public interface ICartService
    {
        CartViewModel GetUserCart(string userId);

        double GetCartTotalPrice(int cartId);
        
        bool IsThisProductWithThisSizeInCart(int cartId, int productId, ProductSize size);

        IEnumerable<CartProductsQueryModel> GetCartProductsForCart(int cartId);

        void CheckCartProductsForOrderedQuantities(int cartId);

        void EditCartProductQuantity(int id, int quantity);

        void EditCartProductSize(int id, ProductSize size, CartViewModel currentUserCart);

        void DeleteCartProduct(int cartProductId);
    }
}
