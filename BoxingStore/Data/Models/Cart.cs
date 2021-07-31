namespace BoxingStore.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Cart
    {
        public int Id { get; set; }

        public User User { get; set; }

        public ICollection<CartProduct> CartProducts { get; set; }
    }
}
