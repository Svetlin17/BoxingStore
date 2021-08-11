namespace BoxingStore.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Order
    {
        public Order()
        {
            this.IsCompleated = false;
            this.OrderDate = DateTime.Now;
        }

        public int Id { get; set; }

        [Required]
        public bool IsCompleated { get; set; }

        [Required]
        public DateTime? OrderDate { get; set; }

        [Required]
        public double TotalPrice { get; set; }

        [Required]
        public string ClientName { get; set; }

        [Required]
        public string ClientEmail { get; set; }

        public string ClientPhoneNumber { get; set; }

        [Required]
        public string ClientAddress { get; set; }

        [Required]
        public string UserId { get; set; }

        public User User { get; set; }

        public IEnumerable<OrderProduct> OrderProducts { get; set; }
    }
}
