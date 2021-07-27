namespace BoxingStore.Services.Statistics
{
    using System.Linq;
    using BoxingStore.Data;

    public class StatisticsService : IStatisticsService
    {
        private readonly BoxingStoreDbContext data;

        public StatisticsService(BoxingStoreDbContext data)
            => this.data = data;

        public StatisticsServiceModel Total()
        {
            var totalProducts = this.data.Products.Count();

            return new StatisticsServiceModel
            {
                TotalProducts = totalProducts,
            };
        }
    }
}