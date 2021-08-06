namespace BoxingStore.Controllers
{
    using System.Diagnostics;
    using BoxingStore.Models;
    using BoxingStore.Models.Home;
    using BoxingStore.Services.Statistics;
    using Microsoft.AspNetCore.Mvc;
    using BoxingStore.Services.Home;
    using System.Collections.Generic;

    public class HomeController : Controller
    {
        private readonly IHomeService homeService;
        private readonly IStatisticsService statistics;

        public HomeController(IHomeService homeService, IStatisticsService statistics)
        {
            this.homeService = homeService;
            this.statistics = statistics;
        }

        public IActionResult Index()
        {
            List<ProductIndexViewModel> products = this.homeService.GetLastThreeProducts();

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
