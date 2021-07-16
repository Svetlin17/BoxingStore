namespace BoxingStore.Data.Models
{
    using BoxingStore.Data.Models.Enums;
    using System.ComponentModel.DataAnnotations;

    public class Product
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Brand { get; set; }

        [Required]
        public  ProductSize Size { get; set; }

        [Required]
        public int Price { get; set; }

        public string Description { get; set; }

        [Required]
        public string ImageUrl { get; set; }

        public int CategoryId { get; set; }

        public Category Category { get; init; }
    }
}
