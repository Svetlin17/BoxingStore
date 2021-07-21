namespace BoxingStore.Models.Home
{
    using System.Collections.Generic;

    public class IndexViewModel
    {
        public int TotalProducts { get; init; }

        public List<ProductIndexViewModel> Products { get; init; }
    }
}