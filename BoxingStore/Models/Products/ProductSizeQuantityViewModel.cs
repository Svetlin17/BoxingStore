using BoxingStore.Data.Models.Enums;

namespace BoxingStore.Models.Products
{
    public class ProductSizeQuantityViewModel
    {
        public int Id { get; init; }

        public int ProductId { get; init; }

        public ProductSize Size { get; set; }

        public int Quantity { get; set; }
    }
}
