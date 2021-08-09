namespace BoxingStore.Services.Orders
{
    using BoxingStore.Models;
    using System.Collections.Generic;

    public interface IOrderService
    {
        int Create(OrderFormServiceModel order, string userId, int cartId);

        OrderQueryServiceModel All(string userId, bool isAdmin);

        double GetOrderTotalPrice(int orderId);

        OrderInfoServiceModel FindById(int id);

        IEnumerable<OrderProductsQueryModel> GetOrderProductsForOrder(int orderId);
        void CompleateOrder(int id);

        void DeleteOrder(int id);
    }
}
