namespace BoxingStore.Controllers
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using BoxingStore.Services.Products;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Caching.Memory;

    using static WebConstants.Cache;

    public class HomeController : Controller
    {
        private readonly IProductService products;
        private readonly IMemoryCache cache;

        public HomeController(IProductService products, IMemoryCache cache)
        {
            this.products = products;
            this.cache = cache;
        }

        public IActionResult Index()
        {
            var latestProducts = this.cache.Get<List<LatestProductServiceModel>>(LatestProductsCacheKey);

            if (latestProducts == null) //take from DB only if there is no cache
            {
                latestProducts = this.products
                   .Latest()
                   .ToList();

                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(15)); //save data in the cache for 15 mins

                this.cache.Set(LatestProductsCacheKey, latestProducts, cacheOptions);
            }

            return View(latestProducts);
        }

        public IActionResult Error() => View();
    }
}
