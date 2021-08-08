namespace BoxingStore.Data.Models
{
    using BoxingStore.Data.Models.Enums;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class OrderProduct
    {
        public int Id { get; set; }

        [Required]
        public int OrderId { get; set; }

        public Order Order { get; set; }

        [Required]
        public int ProductId { get; set; }

        public Product Product { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(3)")]  //store enum in DB as a string
        public ProductSize Size { get; set; }

        [Required]
        public int Quantity { get; set; }
    }
}
