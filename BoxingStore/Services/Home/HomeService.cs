namespace BoxingStore.Services.Home
{
    using System.Linq;
    using System.Collections.Generic;
    using BoxingStore.Data;
    using BoxingStore.Models.Home;

    public class HomeService : IHomeService
    {
        private readonly BoxingStoreDbContext data;

        public HomeService(BoxingStoreDbContext data)
            => this.data = data;

        public List<ProductIndexViewModel> GetLastThreeProducts()
            => this.data
                .Products
                .OrderByDescending(p => p.Id)   //to add the last 3 products 
                .Select(p => new ProductIndexViewModel
                {
                    Id = p.Id,
                    Brand = p.Brand,
                    Name = p.Name,
                    Price = p.Price,
                    ImageUrl = p.ImageUrl,
                })
                .Take(3)
                .ToList();
    }
}
