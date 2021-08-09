namespace BoxingStore.Services.Orders
{
    using BoxingStore.Data.Models;
    using System.Collections.Generic;

    public class OrderInfoServiceModel
    {
        public double TotalPrice { get; set; }

        public string ClientName { get; set; }

        public string ClientEmail { get; set; }

        public string ClientPhoneNumber { get; set; }

        public string ClientAddress { get; set; }

        public IEnumerable<OrderProduct> OrderProducts { get; set; }
    }
}
