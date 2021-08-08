namespace BoxingStore.Services.Orders
{
    using BoxingStore.Data.Models;

    public interface IOrderService
    {
        Order Create();

        OrderQueryServiceModel All();
    }
}
