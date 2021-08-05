
namespace BoxingStore.Models
{
    using BoxingStore.Data.Models.Enums;
    
    public class CartProductsQueryModel
    {
        public string ProductConvertedName { get; init; }

        public string ProductName { get; init; }

        public string ProductImageUrl { get; init; }

        public ProductSize Size { get; set; }

        public int Quantity { get; set; }

        public double Price { get; set; }

        public double TotalPrice { get; set; }
    }
}
