
namespace BoxingStore.Models
{
    using BoxingStore.Data.Models.Enums;
    
    public class CartProductsQueryModel
    {
        public ProductSize Size { get; init; }

        public int Quantity { get; init; }
    }
}
