namespace BoxingStore.Models.Products
{
    public class ProductListingViewModel
    {
        public int Id { get; init; }

        public string Brand { get; init; }

        public string Name { get; init; }

        public string ConvertedName { get; init; }

        public double Price { get; init; }

        public string ImageUrl { get; init; }

        public string Category { get; init; }
    }
}
