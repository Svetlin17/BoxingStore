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

        //return the View of the form and visualise what is on it
        public IActionResult Add() => View(new AddProductFormModel 
        {
            Categories = this.GetProductCategories()
        });

        [HttpPost]
        public IActionResult Add(AddProductFormModel product)
        {
            if (!this.data.Categories.Any(c => c.Id == product.CategoryId)) //validation - attributes cant
            {
                this.ModelState.AddModelError(nameof(product.CategoryId), "Category does not exist.");
            }

            if (!ModelState.IsValid)
            {
                product.Categories = this.GetProductCategories(); //to get all categories, not only chosen one

                return View(product); //if there's wrong input refresh the page, valid data remains filled
            }

            if (product.QuantityS >= ProductQuantityMin)
            {
                this.data.Products.Add(CreateNewProduct(product, (int)(ProductSize.S), product.QuantityS));
                this.data.SaveChanges();
            }
            if (product.QuantityM >= ProductQuantityMin)
            {
                this.data.Products.Add(CreateNewProduct(product, (int)(ProductSize.M), product.QuantityM));
                this.data.SaveChanges();
            }
            if (product.QuantityL >= ProductQuantityMin)
            {
                this.data.Products.Add(CreateNewProduct(product, (int)(ProductSize.L), product.QuantityL));
                this.data.SaveChanges();
            }

            return RedirectToAction("Index", "Home"); //must redirect in order not to dublicate data when refreshing
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
