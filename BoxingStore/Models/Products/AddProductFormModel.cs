namespace BoxingStore.Models.Products
{
    using BoxingStore.Data.Models.Enums;
    using System.ComponentModel.DataAnnotations;

    public class AddProductFormModel
    {
        public string Brand { get; init; }

        public string Name { get; init; }

        public ProductSize Size { get; init; }

        public double Price { get; init; }

        public int Quantity { get; init; }

        public string Description { get; init; }

        [Display(Name="Image Url")]
        public string ImageUrl { get; init; }

        public int CategoryId { get; init; }

        public IEnumerable<CarCategoryViewModel> Categories { get; set; }
    }
}