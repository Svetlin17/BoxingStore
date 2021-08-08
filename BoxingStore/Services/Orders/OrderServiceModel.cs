namespace BoxingStore.Services.Orders
{
    using BoxingStore.Data.Models.Enums;
    using System;

    public class OrderServiceModel
    {
        public int Id { get; set; }

        public OrderStatus Status { get; set; }

        public DateTime? OrderDate { get; set; }

        public double TotalPrice { get; set; }

        public string ClientName { get; set; }

        public string ClientEmail { get; set; }

        public string ClientPhoneNumber { get; set; }

        public string ClientAddress { get; set; }
    }
}
