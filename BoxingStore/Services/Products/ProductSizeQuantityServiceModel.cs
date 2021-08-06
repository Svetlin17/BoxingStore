﻿namespace BoxingStore.Services.Products
{
    using BoxingStore.Data.Models;
    using BoxingStore.Data.Models.Enums;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class ProductSizeQuantityServiceModel
    {
        public int Id { get; init; }

        public int ProductId { get; init; }

        public Product Product { get; init; }

        public ProductSize Size { get; set; }

        public int Quantity { get; set; }

        public ICollection<ProductSizeQuantity> SizeQuantities { get; set; }
    }
}
