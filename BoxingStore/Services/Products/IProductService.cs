namespace BoxingStore.Services.Products
{
    using System.Collections.Generic;
    using BoxingStore.Data.Models;
    using BoxingStore.Models;
    using BoxingStore.Service.Products;

    public interface IProductService
    {
        ProductQueryServiceModel All(
            string brand,
            string searchTerm,
            ProductSorting sorting,
            int currentPage,
            int productsPerPage);

        ProductDetailsServiceModel FindById(int Id);

        ProductDetailsServiceModel FindByConvertedName(string convertedName);

        Product Create(ProductFormServiceModel product, string convertedName);
        bool Edit(
                int id,
                string brand,
                string name,
                string description,
                string imageUrl,
                double price,
                int categoryId,
                string convertedName,
                int quantityS,
                int quantityM,
                int quantityL);

        IEnumerable<string> BrandsSorting();

        ICollection<ProductSizeQuantity> ProductSizeQuantity(int productId);

        IEnumerable<ProductCategoryServiceModel> AllCategories();

        Category ProductCategory(string productCategoryName);

        bool CategoryExists(int categoryId);

        string CreateConvertedName(ProductFormServiceModel product);
    }
}