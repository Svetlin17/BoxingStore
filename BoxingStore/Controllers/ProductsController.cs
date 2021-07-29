namespace BoxingStore.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using BoxingStore.Models.Products;
    using BoxingStore.Services.Products;
    using BoxingStore.Data;
    using System.Linq;
    using BoxingStore.Data.Models;
    using BoxingStore.Data.Models.Enums;

    using static Data.DataConstants;

    public class ProductsController : Controller
    {
        private readonly IProductService products;
        private readonly BoxingStoreDbContext data;

        public ProductsController(IProductService products, BoxingStoreDbContext data)
        {
            this.products = products;
            this.data = data;
        }

        public IActionResult All([FromQuery] AllProductsQueryModel query)
        {
            var queryResult = this.products.All(
                query.Brand,
                query.SearchTerm,
                query.Sorting,
                query.CurrentPage,
                AllProductsQueryModel.ProductsPerPage);

            var productBrands = this.products.AllBrands();

            //should not be "init" in the AllProductsQueryModel
            query.Brands = productBrands;
            query.TotalProducts = queryResult.TotalProducts;
            query.Products = queryResult.Products;

            return View(query);
        }

        //return the View of the form and visualise what is on it
        public IActionResult Add() => View(new ProductFormServiceModel
        {
            Categories = this.products.AllCategories() //they are null so initializing them
        });

        [HttpPost]
        public IActionResult Add(ProductFormServiceModel product)
        {
            if (!this.products.CategoryExists(product.CategoryId)) //validation - attributes cant
            {
                this.ModelState.AddModelError(nameof(product.CategoryId), "Category does not exist.");
            }

            if (!ModelState.IsValid)
            {
                product.Categories = this.products.AllCategories(); //to get all categories, not only chosen one

                return View(product); //if there's wrong input refresh the page, valid data remains filled
            }

            string convertedName = this.products.CreateConvertedName(product);

            /* TODO
            if (this.data.Products.Any(p => p.ConvertedName == convertedName)) //validates that the converted name is unique
            {
                this.ModelState.AddModelError(nameof(product.Name), "A product with that name already exists.");
            }
            */

            this.data.Products.Add(this.products.Create(product, convertedName));
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

            return RedirectToAction(nameof(All)); //must redirect in order not to dublicate data when refreshing
        } 

        //public IActionResult Edit(convertedName)

        //private IEnumerable<ProductSizeQuantityServiceModel> GetProductSizeQuantities()
        //    => this.data
        //        .ProductSizeQuantities
        //        .Select(p => new ProductSizeQuantityServiceModel
        //        {
        //            Id = p.Id,
        //            Size = p.Size,
        //            Quantity = p.Quantity
        //        })
        //        .ToList();


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