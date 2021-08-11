namespace BoxingStore.Services.Orders
{
    using System;

    public class OrderServiceModel
    {
        public int Id { get; set; }

        public bool IsCompleated { get; set; }

        public DateTime? OrderDate { get; set; }

        public double TotalPrice { get; set; }

        public string ClientName { get; set; }

        public string ClientEmail { get; set; }

        public string ClientPhoneNumber { get; set; }

        public string ClientAddress { get; set; }
    }
}
