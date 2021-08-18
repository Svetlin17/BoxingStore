namespace BoxingStore.Test.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using BoxingStore.Data.Models;

    public static class Products
    {
        public static IEnumerable<Product> TenPublicProducts
            => Enumerable.Range(0, 10).Select(i => new Product
            {                
            });
    }
}