namespace BoxingStore.Services.Orders
{
    public interface IOrderService
    {
        int Create(OrderFormServiceModel order, string userId, int cartId);

        OrderQueryServiceModel All();

        double GetOrderTotalPrice(int orderId);

        OrderInfoServiceModel FindById(int id);
    }
}
