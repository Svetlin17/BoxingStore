namespace BoxingStore.Controllers
{
    using System.Diagnostics;
    using BoxingStore.Models;
    using BoxingStore.Models.Home;
    using BoxingStore.Services.Statistics;
    using BoxingStore.Services.Products;
    using Microsoft.AspNetCore.Mvc;
    using System.Collections.Generic;
    using System.Linq;

    public class HomeController : Controller
    {
        private readonly IProductService products;
        private readonly IStatisticsService statistics;

        public HomeController(IProductService products, IStatisticsService statistics)
        {
            this.products = products;
            this.statistics = statistics;
        }

        public IActionResult Index()
        {
            List<LatestProductServiceModel> latestProducts = this.products.Latest().ToList();

            var totalStatistics = this.statistics.Total();

            return View(new IndexViewModel
            {
                TotalProducts = totalStatistics.TotalProducts,
                Products = latestProducts
            });
        }

        public IActionResult Error() => View();
    }
}
