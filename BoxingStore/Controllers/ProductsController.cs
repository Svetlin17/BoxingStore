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
    using System;

    public class ProductsController : Controller
    {
        private readonly BoxingStoreDbContext data;

        public ProductsController(BoxingStoreDbContext data)
            => this.data = data;


        public IActionResult All([FromQuery] AllProductsQueryModel query) //by def classes dont bind from get request so we need to say explicitly to get it from query
        {
            var productsQuery = this.data.Products.AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.Brand))
            {
                productsQuery = productsQuery.Where(p => p.Brand == query.Brand);
            }

            if (!string.IsNullOrWhiteSpace(query.SearchTerm))
            {
                productsQuery = productsQuery.Where(p =>
                    (p.Brand + " " + p.Name).ToLower().Contains(query.SearchTerm.ToLower()) ||
                    p.Description.ToLower().Contains(query.SearchTerm.ToLower()));
            }

            productsQuery = query.Sorting switch
            {
                ProductSorting.BrandAndModel => productsQuery.OrderBy(p => p.Brand).ThenBy(p => p.Name),
                ProductSorting.LastAdded => productsQuery.OrderByDescending(c => c.Id),
                ProductSorting.FirstAdded or _ => productsQuery.OrderBy(c => c.Id)
            };

            var totalProducts = productsQuery.Count();

            var products = productsQuery
                .Skip((query.CurrentPage - 1) * AllProductsQueryModel.ProductsPerPage) //if page 2 - skip products from page 1
                .Take(AllProductsQueryModel.ProductsPerPage)
                .Select(p => new ProductListingViewModel
                {
                    Id = p.Id,
                    Brand = p.Brand,
                    Name = p.Name,
                    ConvertedName = p.ConvertedName,
                    Price = p.Price,
                    ImageUrl = p.ImageUrl,
                    Category = p.Category.Name
                })
                .ToList();

            var productBrands = this.data
                .Products
                .Select(p => p.Brand)
                .Distinct()
                .OrderBy(br => br)
                .ToList();

            //should not be "init" in the AllProductsQueryModel
            query.TotalProducts = totalProducts;
            query.Brands = productBrands;
            query.Products = products;

            return View(query);
        }


        //return the View of the form and visualise what is on it
        public IActionResult Add() => View(new AddProductFormModel
        {
            Categories = this.GetProductCategories() //they are null so initializing them
        });

        [HttpPost]
        public IActionResult Add(AddProductFormModel product)
        {
            if (!this.data.Categories.Any(p => p.Id == product.CategoryId)) //validation - attributes cant
            {
                this.ModelState.AddModelError(nameof(product.CategoryId), "Category does not exist.");
            }

            if (!ModelState.IsValid)
            {
                product.Categories = this.GetProductCategories(); //to get all categories, not only chosen one

                return View(product); //if there's wrong input refresh the page, valid data remains filled
            }

            string convertedName = product.Brand.ToLower();
            string[] nameWords = product.Name.Split(' ');
            foreach (var word in nameWords)
            {
                convertedName += "-" + word.ToLower();
            }

            /* TODO
            if (this.data.Products.Any(p => p.ConvertedName == convertedName)) //validates that the converted name is unique
            {
                this.ModelState.AddModelError(nameof(product.Name), "A product with that name already exists.");
            }
            */

            this.data.Products.Add(CreateNewProduct(product, convertedName));
            this.data.SaveChanges();

            if (product.QuantityS >= ProductQuantityMin)
            {
                this.data.ProductSizeQuantities.Add(CreateProductSizeQuantity(ProductSize.S, product.QuantityS, convertedName));
                this.data.SaveChanges();
            }
            if (product.QuantityM >= ProductQuantityMin)
            {
                this.data.ProductSizeQuantities.Add(CreateProductSizeQuantity(ProductSize.M, product.QuantityM, convertedName));
                this.data.SaveChanges();
            }
            if (product.QuantityL >= ProductQuantityMin)
            {
                this.data.ProductSizeQuantities.Add(CreateProductSizeQuantity(ProductSize.L, product.QuantityL, convertedName));
                this.data.SaveChanges();
            }

            //must redirect in order not to dublicate data when refreshing
            return RedirectToAction(nameof(All));   // (action, controller)
        }

        private IEnumerable<ProductCategoryViewModel> GetProductCategories()
            => this.data
                .Categories
                .Select(p => new ProductCategoryViewModel
                {
                    Id = p.Id,
                    Name = p.Name
                })
                .ToList();

        private IEnumerable<ProductSizeQuantityViewModel> GetProductSizeQuantities()
            => this.data
                .ProductSizeQuantities
                .Select(p => new ProductSizeQuantityViewModel
                {
                    Id = p.Id,
                    Size = p.Size,
                    Quantity = p.Quantity
                })
                .ToList();

        private Product CreateNewProduct(AddProductFormModel product, string convertedName)
        {
            var productData = new Product
            {
                Brand = product.Brand,
                Name = product.Name,
                ConvertedName = convertedName,
                Price = product.Price,
                Description = product.Description,
                ImageUrl = product.ImageUrl,
                CategoryId = product.CategoryId
            };

            return productData;
        }

        private ProductSizeQuantity CreateProductSizeQuantity(ProductSize size, int quantity, string convertedName)
        {
            var product = this.data.Products.Where(x => x.ConvertedName == convertedName).FirstOrDefault();

            var productData = new ProductSizeQuantity
            {
                ProductId = product.Id,
                Size = (ProductSize)size,
                Quantity = quantity
            };

            return productData;
        }
    }
}