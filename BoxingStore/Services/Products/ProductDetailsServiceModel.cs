namespace BoxingStore.Services.Products
{
    using BoxingStore.Data.Models;
    using System.Collections.Generic;
    
    public class ProductDetailsServiceModel : ProductServiceModel
    {
        public string Description { get; init; }

        public ICollection<ProductSizeQuantity> SizeQuantities { get; set; }

        public bool NoteAfterOrder { get; set; }
    }
}
