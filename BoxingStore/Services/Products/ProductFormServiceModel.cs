using BoxingStore.Service.Products;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using static BoxingStore.Data.DataConstants;

namespace BoxingStore.Services.Products
{
    public class ProductFormServiceModel
    {
        [Required]
        public string Brand { get; init; }

        [Required]
        public string Name { get; init; }

        [Range(ProductPriceMin, Int32.MaxValue, ErrorMessage = ProductPriceMsg)]
        public double Price { get; init; }

        //[Range(ProductQuantityMin, Int32.MaxValue, ErrorMessage = "Quantity should be at least 1")]
        public int QuantityS { get; init; }

        //[Range(ProductQuantityMin, Int32.MaxValue, ErrorMessage = "Quantity should be at least 1")]
        public int QuantityM { get; init; }

        //[Range(ProductQuantityMin, Int32.MaxValue, ErrorMessage = "Quantity should be at least 1")]
        public int QuantityL { get; init; }

        [Required]
        public string Description { get; init; }

        [Display(Name = "Image Url")]
        [Required]
        [Url]
        public string ImageUrl { get; init; }

        [Display(Name = "Category")]
        public int CategoryId { get; init; }

        public IEnumerable<ProductCategoryServiceModel> Categories { get; set; }

        public IEnumerable<ProductSizeQuantityServiceModel> ProductSizeQuantities { get; set; }
    }
}
