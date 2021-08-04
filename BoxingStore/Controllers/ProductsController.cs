namespace BoxingStore.Controllers
{
    using AutoMapper;
    using BoxingStore.Data;
    using BoxingStore.Data.Models;
    using BoxingStore.Data.Models.Enums;
    using BoxingStore.Models.Products;
    using BoxingStore.Services.Products;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System.Collections.Generic;
    using System.Linq;

    using static Data.DataConstants;

    public class ProductsController : Controller
    {
        private readonly IProductService products;
        private readonly BoxingStoreDbContext data;
        private readonly IMapper mapper;

        public ProductsController(IProductService products, BoxingStoreDbContext data, IMapper mapper)
        {
            this.products = products;
            this.data = data;
            this.mapper = mapper;
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

            query.Brands = productBrands; //should not be "init"
            query.TotalProducts = queryResult.TotalProducts;
            query.Products = queryResult.Products;

            return View(query);
        }

        //return the View of the form and visualise what is on it
        [Authorize(Roles = WebConstants.AdministratorRoleName)]
        public IActionResult Add() => View(new ProductFormServiceModel
        {
            Categories = this.products.AllCategories() //they are null so initializing them
        });

        [HttpPost]
        [Authorize(Roles = WebConstants.AdministratorRoleName)]
        public IActionResult Add(ProductFormServiceModel product)
        {
            if (!this.products.CategoryExists(product.CategoryId)) //validation - attributes cant
            {
                this.ModelState.AddModelError(nameof(product.CategoryId), "Category does not exist.");
            }

            string convertedName = this.products.CreateConvertedName(product);

            if (this.data.Products.Any(p => p.ConvertedName == convertedName)) //validates that the converted name is unique
            {
                product.Categories = this.products.AllCategories();

                this.ModelState.AddModelError(nameof(product.Name), "A product with that name already exists.");

                return View(product);
            }

            if (!ModelState.IsValid)
            {
                product.Categories = this.products.AllCategories(); //to get all categories, not only chosen one

                return View(product); //if there's wrong input refresh the page, valid data remains filled
            }

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

        [Authorize(Roles = WebConstants.AdministratorRoleName)]
        public IActionResult Edit(int id)
        {
            var product = this.products.FindById(id);

            ICollection<ProductSizeQuantity> allSizesForCurrentProduct = this.data.ProductSizeQuantities.Where(p => p.ProductId == product.Id).ToList();

            return View(new ProductFormServiceModel
            {
                Brand = product.Brand,
                Name = product.Name,
                ImageUrl = product.ImageUrl,
                Description = product.Description,
                Price = product.Price,
                CategoryId = product.CategoryId,
                Categories = this.products.AllCategories(),
                QuantityS = allSizesForCurrentProduct.Where(x => x.Size == ProductSize.S).FirstOrDefault().Quantity,
                QuantityM = allSizesForCurrentProduct.Where(x => x.Size == ProductSize.M).FirstOrDefault().Quantity,
                QuantityL = allSizesForCurrentProduct.Where(x => x.Size == ProductSize.L).FirstOrDefault().Quantity,
            });
        }

        [HttpPost]
        [Authorize(Roles = WebConstants.AdministratorRoleName)]
        public IActionResult Edit(int id, ProductFormServiceModel product)
        {
            if (!this.products.CategoryExists(product.CategoryId))
            {
                this.ModelState.AddModelError(nameof(product.CategoryId), "Category does not exist.");
            }

            if (!ModelState.IsValid)
            {
                product.Categories = this.products.AllCategories();

                return View(product);
            }

            string newConvertedName = this.products.CreateConvertedName(product);

            var productIsEdited = this.products.Edit(
                id,
                product.Brand,
                product.Name,
                product.Description,
                product.ImageUrl,
                product.Price,
                product.CategoryId,
                newConvertedName,
                product.QuantityS,
                product.QuantityM,
                product.QuantityL);

            if (!productIsEdited)
            {
                return BadRequest();
            }

            return RedirectToAction(nameof(All));
        }

        public IActionResult Details(int id)
        {
            var product = this.products.FindById(id);

            var productCategory = this.data.Categories.FirstOrDefault(c => c.Name == product.CategoryName);

            ICollection<ProductSizeQuantity> allSizesForCurrentProduct = this.data.ProductSizeQuantities.Where(p => p.ProductId == product.Id).ToList();

            return View(new ProductDetailsServiceModel
            {
                Id = product.Id,
                Brand = product.Brand,
                Name = product.Name,
                ImageUrl = product.ImageUrl,
                Description = product.Description,
                Price = product.Price,
                CategoryId = productCategory.Id,
                CategoryName = product.CategoryName,
                SizeQuantities = allSizesForCurrentProduct
            });
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