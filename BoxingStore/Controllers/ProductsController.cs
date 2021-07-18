namespace BoxingStore.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using BoxingStore.Models.Products;
    using System.Collections.Generic;
    using BoxingStore.Data;
    using System.Linq;
    using BoxingStore.Data.Models;

    public class ProductsController : Controller
    {
        private readonly BoxingStoreDbContext data;

        public ProductsController(BoxingStoreDbContext data)
            => this.data = data;

        public IActionResult Add() => View(new AddProductFormModel
        {
            Categories = this.GetProductCategories()
        });

        [HttpPost]
        public IActionResult Add(AddProductFormModel product)
        {
            if (!this.data.Categories.Any(c => c.Id == product.CategoryId))
            {
                this.ModelState.AddModelError(nameof(product.CategoryId), "Category does not exist.");
            }

            if (!ModelState.IsValid)
            {
                product.Categories = this.GetProductCategories();

                return View(product);
            }

            var productData = new Product
            {
                Brand = product.Brand,
                Name = product.Name,
                Price = product.Price,
                Size = product.Size,
                Quantity = product.Quantity,
                Description = product.Description,
                ImageUrl = product.ImageUrl,
                CategoryId = product.CategoryId
            };

            this.data.Products.Add(productData);
            this.data.SaveChanges();

            return RedirectToAction("Index", "Home");
        }

        private IEnumerable<ProductCategoryViewModel> GetProductCategories()
            => this.data
                .Categories
                .Select(c => new ProductCategoryViewModel
                {
                    Id = c.Id,
                    Name = c.Name
                })
                .ToList();
    }
}
