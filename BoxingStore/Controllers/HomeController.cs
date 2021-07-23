namespace BoxingStore.Controllers
{
    using System.Linq;
    using System.Diagnostics;
    using BoxingStore.Data;
    using BoxingStore.Models;
    using BoxingStore.Models.Home;
    using Microsoft.AspNetCore.Mvc;

    public class HomeController : Controller
    {
        private readonly BoxingStoreDbContext data;

        public HomeController(BoxingStoreDbContext data)
            => this.data = data;

        public IActionResult Index()
        {
            var totalProducts = this.data.Products.Count();

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

            return View(new IndexViewModel
            {
                TotalProducts = totalProducts,
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
