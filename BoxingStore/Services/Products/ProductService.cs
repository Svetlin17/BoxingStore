﻿namespace BoxingStore.Services.Products
{
    using System.Collections.Generic;
    using System.Linq;
    using BoxingStore.Models;
    using BoxingStore.Data;
    using BoxingStore.Data.Models;
    using BoxingStore.Service.Products;
    using BoxingStore.Data.Models.Enums;
    using AutoMapper.QueryableExtensions;
    using AutoMapper;

    public class ProductService : IProductService
    {
        private readonly BoxingStoreDbContext data;
        private readonly IConfigurationProvider mapper;

        public ProductService(BoxingStoreDbContext data, IMapper mapper)
        { 
            this.data = data;
            this.mapper = mapper.ConfigurationProvider;
        } 

        public int Create(ProductFormServiceModel product, string convertedName)
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

            this.data.Products.Add(productData);
            this.data.SaveChanges();

            return productData.Id;
        }

        public bool Edit(int id, string brand, string name, string description, string imageUrl, double price, int categoryId, string convertedName, int quantityS, int quantityM, int quantityL)
        {
            var productData = this.data.Products.Find(id);

            productData.Brand = brand;
            productData.Name = name;
            productData.Description = description;
            productData.ImageUrl = imageUrl;
            productData.Price = price;
            productData.CategoryId = categoryId;
            productData.ConvertedName = convertedName;

            var productDataQuantities = this.data.ProductSizeQuantities.Where(p => p.ProductId == id).ToList();

            productDataQuantities.Where(x => x.Size == ProductSize.S).FirstOrDefault().Quantity = quantityS;
            productDataQuantities.Where(x => x.Size == ProductSize.M).FirstOrDefault().Quantity = quantityM;
            productDataQuantities.Where(x => x.Size == ProductSize.L).FirstOrDefault().Quantity = quantityL;

            this.data.SaveChanges();

            return true;
        }

        public IEnumerable<LatestProductServiceModel> Latest()
            => this.data
                .Products
                .OrderByDescending(p => p.Id)  
                .ProjectTo<LatestProductServiceModel>(this.mapper)
                //.Select(p => new LatestProductServiceModel
                //{
                //    Id = p.Id,
                //    Brand = p.Brand,
                //    Name = p.Name,
                //    Price = p.Price,
                //    ImageUrl = p.ImageUrl,
                //})
                .Take(3)
                .ToList();

        public ProductQueryServiceModel All(
            string brand,
            string searchTerm,
            ProductSorting sorting,
            int currentPage,
            int productsPerPage)
        {
            var productsQuery = this.data.Products.AsQueryable();

            if (!string.IsNullOrWhiteSpace(brand))
            {
                productsQuery = productsQuery.Where(c => c.Brand == brand);
            }

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                productsQuery = productsQuery.Where(p =>
                    (p.Brand + " " + p.Name).ToLower().Contains(searchTerm.ToLower()) ||
                    p.Description.ToLower().Contains(searchTerm.ToLower()));
            }

            productsQuery = sorting switch
            {
                ProductSorting.BrandAndModel => productsQuery.OrderBy(p => p.Brand).ThenBy(p => p.Name),
                ProductSorting.LastAdded => productsQuery.OrderByDescending(p => p.Id),
                ProductSorting.FirstAdded => productsQuery.OrderBy(p => p.Id),
                ProductSorting.TheMostExpensive => productsQuery.OrderByDescending(p => p.Price),
                ProductSorting.TheCheapest or _ => productsQuery.OrderBy(p => p.Price),
            };

            var totalProducts = productsQuery.Count();

            var products = productsQuery
                .Skip((currentPage - 1) * productsPerPage)
                .Take(productsPerPage)
                .Select(p => new ProductServiceModel
                {
                    Id = p.Id,
                    Brand = p.Brand,
                    Name = p.Name,
                    ConvertedName = p.ConvertedName,
                    Price = p.Price,
                    ImageUrl = p.ImageUrl,
                    CategoryName = p.Category.Name
                })
                .ToList();

            return new ProductQueryServiceModel
            {
                TotalProducts = totalProducts,
                CurrentPage = currentPage,
                ProductsPerPage = productsPerPage,
                Products = products
            };
        }
        public ProductDetailsServiceModel FindById(int id)
            => this.data
            .Products
            .Where(p => p.Id == id)
            .Select(p => new ProductDetailsServiceModel
            {
                Id = p.Id,
                Brand = p.Brand,
                Name = p.Name,
                ConvertedName = p.ConvertedName,
                Description = p.Description,
                Price = p.Price,
                ImageUrl = p.ImageUrl,
                CategoryId = p.CategoryId,
                CategoryName = p.Category.Name
            })
            .FirstOrDefault();

        public ProductDetailsServiceModel FindByConvertedName(string convertedName)
            => this.data
            .Products
            .Where(p => p.ConvertedName == convertedName)
            .Select(p => new ProductDetailsServiceModel
            {
                Id = p.Id,
                Brand = p.Brand,
                Name = p.Name,
                ConvertedName = p.ConvertedName,
                Description = p.Description,
                Price = p.Price,
                ImageUrl = p.ImageUrl,
                CategoryId = p.CategoryId,
                CategoryName = p.Category.Name
            })
            .FirstOrDefault();

        public IEnumerable<string> BrandsSorting()
            => this.data
                .Products
                .Select(p => p.Brand)
                .Distinct()
                .OrderBy(br => br)
                .ToList();

        public ICollection<ProductSizeQuantity> ProductSizeQuantity(int productId)
            => this.data.ProductSizeQuantities.Where(p => p.ProductId == productId).ToList();

        public IEnumerable<ProductCategoryServiceModel> AllCategories()
        => this.data
                .Categories
                .Select(p => new ProductCategoryServiceModel
                {
                    Id = p.Id,
                    Name = p.Name
                })
                .ToList();

        public Category ProductCategory(string productCategoryName)
            => this.data.Categories.FirstOrDefault(c => c.Name == productCategoryName);

        public bool CategoryExists(int categoryId)
        => this.data
            .Categories
            .Any(p => p.Id == categoryId);

        public string CreateConvertedName(ProductFormServiceModel product)
        {
            string convertedName = product.Brand.ToLower();
            string[] nameWords = product.Name.Split(' ');
            foreach (var word in nameWords)
            {
                convertedName += "-" + word.ToLower();
            }

            return convertedName;
        }
    }
}