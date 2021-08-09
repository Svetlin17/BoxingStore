namespace BoxingStore.Services.Orders
{
    using BoxingStore.Models;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class OrderFormServiceModel
    {
        [Display(Name = "Status")]
        public bool IsCompleated { get; set; }

        [Required]
        [Display(Name = "Total Price")]
        public double TotalPrice { get; set; }

        [Required]
        [Display(Name = "Name")]
        public string ClientName { get; set; }

        [Required]
        [Display(Name = "Email")]
        public string ClientEmail { get; set; }

        [Required]
        [Display(Name = "Phone")]
        public string ClientPhoneNumber { get; set; }

        [Required]
        [Display(Name = "Address")]
        public string ClientAddress { get; set; }
        
        public IEnumerable<OrderProductsQueryModel> OrderProducts { get; set; }
    }
}
