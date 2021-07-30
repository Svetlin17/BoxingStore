using BoxingStore.Data.Models;
using BoxingStore.Service.Products;
using BoxingStore.Services.CustomAttributes;
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

        [Range(ProductPriceMin, Int32.MaxValue, ErrorMessage = ProductPriceErrMsg)]
        public double Price { get; init; }

        [ProductSizeAttribute(ErrorMessage = ProductQuantityErrMsg)]
        public int QuantityS { get; init; }

        [ProductSizeAttribute(ErrorMessage = ProductQuantityErrMsg)]
        public int QuantityM { get; init; }

        [ProductSizeAttribute(ErrorMessage = ProductQuantityErrMsg)]
        public int QuantityL { get; init; }

        //public ICollection<ProductSizeQuantity> ProductSizeQuantities { get; set; }

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
