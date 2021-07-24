using BoxingStore.Data.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BoxingStore.Data.Models
{
    public class ProductSizeQuantity
    {
        public int Id { get; init; }

        [Required]
        public int ProductId { get; init; }

        public Product Product { get; init; }

        [Required]
        [Column(TypeName = "nvarchar(3)")]  //store enum in DB as a string
        public ProductSize Size { get; set; }

        [Required]
        public int Quantity { get; set; }
    }
}
