namespace BoxingStore.Services.Home
{
    using BoxingStore.Models.Home;
    using System.Collections.Generic;

    public interface IHomeService
    {
        List<ProductIndexViewModel> GetLastThreeProducts();
    }
}
