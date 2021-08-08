namespace BoxingStore.Models.Orders
{
    using BoxingStore.Services.Orders;
    using System.Collections.Generic;

    public class AllOrdersQueryModel
    {
        public IEnumerable<OrderServiceModel> Orders { get; set; }
    }
}
