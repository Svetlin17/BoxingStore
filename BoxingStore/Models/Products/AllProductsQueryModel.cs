namespace BoxingStore.Models.Products
{
    using BoxingStore.Services.Products;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using static Data.DataConstants;

    public class AllProductsQueryModel
    {
        public const int ProductsPerPage = ProductsPerPageNumber;

        public string Brand { get; init; }

        public string Category { get; init; }

        [Display(Name = "Search by text")]
        public string SearchTerm { get; init; }

        public ProductSorting Sorting { get; init; }

        public int CurrentPage { get; init; } = 1;

        public int TotalProducts { get; set; }

        public IEnumerable<string> Brands { get; set; }

        public IEnumerable<string> Categories { get; set; }

        public IEnumerable<ProductServiceModel> Products { get; set; }
    }
}
