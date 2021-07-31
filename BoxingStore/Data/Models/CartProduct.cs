namespace BoxingStore.Data.Models
{
    using BoxingStore.Data.Models.Enums;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class CartProduct
    {
        public int Id { get; set; }

        [Required]
        public int CartId { get; set; }

        public Cart Cart { get; set; }

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
