namespace BoxingStore.Controllers
{
    using System.Linq;
    using System.Diagnostics;
    using BoxingStore.Data;
    using BoxingStore.Models;
    using BoxingStore.Models.Home;
    using BoxingStore.Services.Statistics;
    using Microsoft.AspNetCore.Mvc;

    public class HomeController : Controller
    {
        private readonly IStatisticsService statistics;
        private readonly BoxingStoreDbContext data;

        public HomeController(
            IStatisticsService statistics,
            BoxingStoreDbContext data)
        {
            this.statistics = statistics;
            this.data = data;
        }

        public IActionResult Index()
        {
            var products = this.data
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

            var totalStatistics = this.statistics.Total();

            return View(new IndexViewModel
            {
                TotalProducts = totalStatistics.TotalProducts,
                Products = products
            });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
