namespace BoxingStore.Services.Orders
{
    using BoxingStore.Models;
    using System;
    using System.Collections.Generic;

    public class OrderInfoServiceModel
    {
        public DateTime? OrderDate { get; set; }

        public bool IsCompleated { get; set; }

        public double TotalPrice { get; set; }

        public string ClientName { get; set; }

        public string ClientEmail { get; set; }

        public string ClientPhoneNumber { get; set; }

        public string ClientAddress { get; set; }

        public IEnumerable<OrderProductsQueryModel> OrderProducts { get; set; }
    }
}
