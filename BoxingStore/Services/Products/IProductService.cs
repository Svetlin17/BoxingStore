namespace BoxingStore.Services.Products
{
    using System.Collections.Generic;
    using BoxingStore.Models;

    public interface IProductService
    {
        ProductQueryServiceModel All(
            string brand,
            string searchTerm,
            ProductSorting sorting,
            int currentPage,
            int productsPerPage);

        IEnumerable<string> AllProductBrands();
    }
}