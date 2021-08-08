namespace BoxingStore.Services.Orders
{
    using System.Collections.Generic;

    public class OrderQueryServiceModel
    {
        public IEnumerable<OrderServiceModel> Orders { get; set; }
    }
}
