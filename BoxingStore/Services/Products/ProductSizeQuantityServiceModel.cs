﻿namespace BoxingStore.Services.Products
{
    using BoxingStore.Data.Models.Enums;

    public class ProductSizeQuantityServiceModel
    {
        public int Id { get; init; }

        public int ProductId { get; init; }

        public ProductSize Size { get; set; }

        public int Quantity { get; set; }
    }
}
