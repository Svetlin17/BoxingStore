namespace BoxingStore.Models.Products
{
    using BoxingStore.Data.Models.Enums;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using static Data.DataConstants;

    public class AddProductFormModel
    {
        [Required]
        public string Brand { get; init; }

        [Required]
        public string Name { get; init; }

        //[Required]
        //public ProductSize Size { get; init; }

        [Range(ProductPriceMin, Int32.MaxValue, ErrorMessage = "Price cannot be 0$")]
        public double Price { get; init; }

        //[Range(ProductQuantityMin, Int32.MaxValue, ErrorMessage = "Quantity should be at least 1")]
        public int QuantityXS { get; init; }

        //[Range(ProductQuantityMin, Int32.MaxValue, ErrorMessage = "Quantity should be at least 1")]
        public int QuantityS { get; init; }

        //[Range(ProductQuantityMin, Int32.MaxValue, ErrorMessage = "Quantity should be at least 1")]
        public int QuantityM { get; init; }

        //[Range(ProductQuantityMin, Int32.MaxValue, ErrorMessage = "Quantity should be at least 1")]
        public int QuantityL { get; init; }

        //[Range(ProductQuantityMin, Int32.MaxValue, ErrorMessage = "Quantity should be at least 1")]
        public int QuantityXL { get; init; }

        [Required]
        public string Description { get; init; }

        [Display(Name="Image Url")]
        [Required]
        [Url]
        public string ImageUrl { get; init; }

        [Display(Name = "Category")]
        public int CategoryId { get; init; }

        public IEnumerable<ProductCategoryViewModel> Categories { get; set; }
    }
}