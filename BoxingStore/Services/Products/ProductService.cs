namespace BoxingStore.Services.Products
{
    using System.Collections.Generic;
    using System.Linq;
    using BoxingStore.Models;
    using BoxingStore.Data;
    using BoxingStore.Data.Models;
    using BoxingStore.Service.Products;

    public class ProductService : IProductService
    {
        private readonly BoxingStoreDbContext data;

        public ProductService(BoxingStoreDbContext data)
            => this.data = data;

        public Product Create(ProductFormServiceModel product, string convertedName)
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

        public bool Edit(int id, string brand, string name, string description, string imageUrl, double price, int categoryId)
        {
            var carData = this.data.Products.Find(id);

            carData.Brand = brand;
            carData.Name = name;
            carData.Description = description;
            carData.ImageUrl = imageUrl;
            carData.Price = price;
            carData.CategoryId = categoryId;

            this.data.SaveChanges();

            return true;
        }

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
                ProductSorting.FirstAdded or _ => productsQuery.OrderBy(p => p.Id)
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
        public ProductDetailsServiceModel Details(int id)
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
                CategoryName = p.Category.Name
            })
            .FirstOrDefault();

        public IEnumerable<string> AllBrands()
            => this.data
                .Products
                .Select(c => c.Brand)
                .Distinct()
                .OrderBy(br => br)
                .ToList();

        public IEnumerable<ProductCategoryServiceModel> AllCategories()
        => this.data
                .Categories
                .Select(p => new ProductCategoryServiceModel
                {
                    Id = p.Id,
                    Name = p.Name
                })
                .ToList();

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