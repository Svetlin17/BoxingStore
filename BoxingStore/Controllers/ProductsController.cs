namespace BoxingStore.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using BoxingStore.Models.Products;
    using System.Collections.Generic;
    using BoxingStore.Data;
    using System.Linq;
    using BoxingStore.Data.Models;
    using BoxingStore.Data.Models.Enums;

    using static Data.DataConstants;

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

            if (product.QuantityXS >= ProductQuantityMin)
            {
                this.data.Products.Add(CreateNewProduct(product, 0, product.QuantityXS));
                this.data.SaveChanges();
            }
            if (product.QuantityS >= ProductQuantityMin)
            {
                this.data.Products.Add(CreateNewProduct(product, 1, product.QuantityS));
                this.data.SaveChanges();
            }
            if (product.QuantityM >= ProductQuantityMin)
            {
                this.data.Products.Add(CreateNewProduct(product, 2, product.QuantityM));
                this.data.SaveChanges();
            }
            if (product.QuantityL >= ProductQuantityMin)
            {
                this.data.Products.Add(CreateNewProduct(product, 3, product.QuantityL));
                this.data.SaveChanges();
            }
            if (product.QuantityXL >= ProductQuantityMin)
            {
                this.data.Products.Add(CreateNewProduct(product, 4, product.QuantityXL));
                this.data.SaveChanges();
            }

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

        private Product CreateNewProduct(AddProductFormModel product, int size, int quantity)
        {
            var productData = new Product
            {
                Brand = product.Brand,
                Name = product.Name,
                Price = product.Price,
                Size = (ProductSize)size,
                Quantity = quantity,
                Description = product.Description,
                ImageUrl = product.ImageUrl,
                CategoryId = product.CategoryId
            };

            return productData;
        }
    }
}
