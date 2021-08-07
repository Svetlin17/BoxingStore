namespace BoxingStore.Models.Home
{
    using BoxingStore.Services.Products;
    using System.Collections.Generic;

    public class IndexViewModel
    {
        public int TotalProducts { get; init; }

        public IList<LatestProductServiceModel> Products { get; init; }
    }
}