namespace BoxingStore.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using BoxingStore.Models.Products;

    public class ProductsController : Controller
    {
        public IActionResult Add() => View();

        [HttpPost]
        public IActionResult Add(AddProductFormModel product)
        {


            return View();
        }
    }
}
