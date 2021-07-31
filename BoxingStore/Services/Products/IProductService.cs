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

        ProductDetailsServiceModel Details(int Id);

        Product Create(ProductFormServiceModel product, string convertedName);
        bool Edit(
                int id,
                string brand,
                string name,
                string description,
                string imageUrl,
                double price,
                int categoryId);

        IEnumerable<string> AllBrands();

        IEnumerable<ProductCategoryServiceModel> AllCategories();

        bool CategoryExists(int categoryId);

        string CreateConvertedName(ProductFormServiceModel product);
    }
}