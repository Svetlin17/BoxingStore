namespace BoxingStore.Services.Products
{
    using System.Collections.Generic;
    using BoxingStore.Data.Models;
    using BoxingStore.Data.Models.Enums;
    using BoxingStore.Models;
    using BoxingStore.Service.Products;

    public interface IProductService
    {
        ProductQueryServiceModel All(
            string brand,
            int categoryId,
            string searchTerm,
            ProductSorting sorting,
            int currentPage,
            int productsPerPage);

        IEnumerable<LatestProductServiceModel> Latest();

        Product Find(int id);

        ProductDetailsServiceModel FindById(int Id);

        ProductDetailsServiceModel FindByConvertedName(string convertedName);

        int Create(ProductFormServiceModel product, string convertedName);

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

        void Remove(Product product);

        void AddProductToCart(CartProduct cartProduct);

        IEnumerable<string> BrandsSorting();

        ICollection<ProductSizeQuantity> ProductSizeQuantity(int productId);

        void RemoveProductSizeQuantities(ProductSizeQuantity psq);

        void AddQuantities(ProductFormServiceModel product, int ProductQuantityMin, int productId);

        IEnumerable<ProductCategoryServiceModel> AllCategories();

        Category ProductCategory(string productCategoryName);

        bool CategoryExists(int categoryId);

        string CreateConvertedName(ProductFormServiceModel product);

        bool ConvertedNameExists(string convertedName);

        int MaxQuantityOfSizeAvailable(int productId, ProductSize size);
    }
}