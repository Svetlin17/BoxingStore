namespace BoxingStore.Models
{
    using BoxingStore.Data.Models.Enums;

    public class OrderProductsQueryModel
    {
        public int Id { get; init; }

        public int ProductId { get; init; }

        public string ProductName { get; init; }

        public string ProductImageUrl { get; init; }

        public ProductSize Size { get; set; }

        public int Quantity { get; set; }

        public double Price { get; set; }

        public double ProductTotalPrice { get; set; }
    }
}
