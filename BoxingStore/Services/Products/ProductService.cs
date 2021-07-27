namespace BoxingStore.Services.Products
{
    using System.Collections.Generic;
    using System.Linq;
    using BoxingStore.Data;
    using BoxingStore.Models;

    public class ProductService : IProductService
    {
        private readonly BoxingStoreDbContext data;

        public ProductService(BoxingStoreDbContext data)
            => this.data = data;

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
                    Category = p.Category.Name
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

        public IEnumerable<string> AllProductBrands()
            => this.data
                .Products
                .Select(c => c.Brand)
                .Distinct()
                .OrderBy(br => br)
                .ToList();
    }
}