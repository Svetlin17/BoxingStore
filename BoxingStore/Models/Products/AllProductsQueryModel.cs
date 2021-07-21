namespace BoxingStore.Models.Products
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class AllProductsQueryModel
    {
        public const int ProductsPerPage = 3;

        public string Brand { get; init; }

        [Display(Name = "Search by text")]
        public string SearchTerm { get; init; }

        public ProductSorting Sorting { get; init; }

        public int CurrentPage { get; init; } = 1;

        public int TotalProducts { get; set; }

        public IEnumerable<string> Brands { get; set; }

        public IEnumerable<ProductListingViewModel> Products { get; set; }
    }
}
