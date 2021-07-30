namespace BoxingStore.Services.Products
{
    public class ProductServiceModel
    {
        public int Id { get; init; }

        public string Brand { get; init; }

        public string Name { get; init; }

        public string ConvertedName { get; init; }

        public double Price { get; init; }

        public string ImageUrl { get; init; }

        public string CategoryName { get; init; }
    }
}
