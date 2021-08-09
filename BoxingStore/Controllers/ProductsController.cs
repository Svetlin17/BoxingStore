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
    using System.Security.Claims;
    using System.Linq;

    using static Data.DataConstants;
    using System;
    using BoxingStore.Services.CartService;

    public class ProductsController : Controller
    {
        private readonly IProductService products;
        private readonly ICartService carts;
        private readonly BoxingStoreDbContext data;
        private readonly IMapper mapper;

        public ProductsController(IProductService products, ICartService carts, BoxingStoreDbContext data, IMapper mapper)
        {
            this.products = products;
            this.carts = carts;
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

            var productBrands = this.products.BrandsSorting();

            query.Brands = productBrands; 
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

            if (product.QuantityS + product.QuantityM + product.QuantityL < 1)
            {
                this.ModelState.AddModelError("allSizesAreZero", AllSizesAreZeroErrMsg);
            }

            if (!ModelState.IsValid)
            {
                product.Categories = this.products.AllCategories(); //to get all categories, not only chosen one

                return View(product); //if there's wrong input refresh the page, valid data remains filled
            }

            var productId = this.products.Create(product, convertedName);

            if (product.QuantityS >= ProductQuantityMin)
            {
                this.data.ProductSizeQuantities.Add(CreateProductSizeQuantity(ProductSize.S, product.QuantityS, productId));
                this.data.SaveChanges();
            }
            if (product.QuantityM >= ProductQuantityMin)
            {
                this.data.ProductSizeQuantities.Add(CreateProductSizeQuantity(ProductSize.M, product.QuantityM, productId));
                this.data.SaveChanges();
            }
            if (product.QuantityL >= ProductQuantityMin)
            {
                this.data.ProductSizeQuantities.Add(CreateProductSizeQuantity(ProductSize.L, product.QuantityL, productId));
                this.data.SaveChanges();
            }

            return RedirectToAction(nameof(All)); //must redirect in order not to dublicate data when refreshing
        }

        [Authorize(Roles = WebConstants.AdministratorRoleName)]
        public IActionResult Edit(int id)
        {
            var product = this.products.FindById(id);

            ICollection<ProductSizeQuantity> allSizesForCurrentProduct = this.products.ProductSizeQuantity(product.Id);

            var productForm = this.mapper.Map<ProductFormServiceModel>(product);

            //shound be "set", not "init"
            productForm.Categories = this.products.AllCategories(); 
            productForm.QuantityS = allSizesForCurrentProduct.Where(x => x.Size == ProductSize.S).FirstOrDefault().Quantity;
            productForm.QuantityM = allSizesForCurrentProduct.Where(x => x.Size == ProductSize.M).FirstOrDefault().Quantity;
            productForm.QuantityL = allSizesForCurrentProduct.Where(x => x.Size == ProductSize.L).FirstOrDefault().Quantity;

            return View(productForm);

            //return View(new ProductFormServiceModel
            //{
            //    Brand = product.Brand,
            //    Categories = this.products.AllCategories(),
            //    QuantityS = allSizesForCurrentProduct.Where(x => x.Size == ProductSize.S).FirstOrDefault().Quantity,
            //});
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

        [Authorize(Roles = WebConstants.AdministratorRoleName)]
        public IActionResult Delete(int id)
        {
            var product = this.data.Products.Find(id);

            ICollection<ProductSizeQuantity> allSizesForCurrentProduct = this.products.ProductSizeQuantity(product.Id);

            foreach (var psq in allSizesForCurrentProduct)
            {
                this.data.ProductSizeQuantities.Remove(psq);
            }

            this.data.Products.Remove(product);

            this.data.SaveChanges();

            return RedirectToAction(nameof(All));
        }


        public IActionResult Details(int id)
        {
            var product = this.products.FindById(id);

            var productCategory = this.products.ProductCategory(product.CategoryName);

            ICollection<ProductSizeQuantity> allSizesForCurrentProduct = this.products.ProductSizeQuantity(product.Id);

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

        [HttpPost]
        public IActionResult Details(ProductSizeQuantityServiceModel productModel)
        {
            var product = this.products.FindById(productModel.Id);

            var currentUserCart = this.carts.GetUserCart(this.User.FindFirstValue(ClaimTypes.NameIdentifier));

            var cartProduct = new CartProduct
            {
                CartId = currentUserCart.Id,
                ProductId = productModel.Id,
                Quantity = productModel.Quantity,
                Size = productModel.Size
            };

            this.data.CartProducts.Add(cartProduct);
            this.data.SaveChanges();

            var productCategory = this.products.ProductCategory(product.CategoryName);

            ICollection<ProductSizeQuantity> allSizesForCurrentProduct = this.products.ProductSizeQuantity(product.Id);

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
                SizeQuantities = allSizesForCurrentProduct,
                NoteAfterOrder = true  //after adding in DB 
            });
        }

        private ProductSizeQuantity CreateProductSizeQuantity(ProductSize size, int quantity, int productId)
        {
            var product = this.data.Products.Find(productId);

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